using AutoMapper;
using Business.Exceptions;
using Business.Interfaces.v1;
using Business.Models.v1.Requests;
using Business.Models.v1.Responses;
using Microsoft.Extensions.Logging;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Persistence.Util;
using static Persistence.Util.Constants;

namespace Business.Implementations.v1;

public class CalculationService : ICalculationService
{
    private readonly IVehicleInfoRepository _vehicleInfoRepository;
    private readonly ITariffTypeRepository _tariffTypeRepository;
    private readonly IEngineVolumeRepository _engineVolumeRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IMunicipalityRepository _municipalityRepository;
    private readonly ICalculationRepository _calculationRepository;
    private readonly IPurposeRepository _purposeRepository;
    private readonly ILogger<CalculationService> _logger;
    private readonly IMapper _mapper;

    public CalculationService(
        IVehicleInfoRepository vehicleInfoRepository,
        ITariffTypeRepository tariffTypeRepository,
        IEngineVolumeRepository engineVolumeRepository,
        IAgeGroupRepository ageGroupRepository,
        IMunicipalityRepository municipalityRepository,
        ICalculationRepository calculationRepository,
        IPurposeRepository purposeRepository,
        ILogger<CalculationService> logger,
        IMapper mapper)
    {
        _vehicleInfoRepository = vehicleInfoRepository;
        _tariffTypeRepository = tariffTypeRepository;
        _engineVolumeRepository = engineVolumeRepository;
        _ageGroupRepository = ageGroupRepository;
        _municipalityRepository = municipalityRepository;
        _calculationRepository = calculationRepository;
        _logger = logger;
        _purposeRepository = purposeRepository;
        _mapper = mapper;
    }

    public async Task<CalculationResponse> CalculateAsync(CalculationRequest request)
    {
        decimal pendingAmount = await _vehicleInfoRepository.GetTotalPendingAmountAsync();
        decimal paidAmount = await _vehicleInfoRepository.GetTotalPaidAmountAsync();
        int totalCount = await _vehicleInfoRepository.GetPolicyCountAsync();

        decimal basePremium = ((pendingAmount + paidAmount) / totalCount) * 1.3M;

        decimal engineVolumeMultiplier = await GetEngineVolumeMultiplierAsync(request.VehicleTariffTypeId);

        decimal ageMultiplier = await GetAgeMultiplierAsync(request.VehiclePurposeId, request.VehicleTypeId, request.OwnerAgeId);

        decimal municipalityMultiplier = await GetMunicipalityMultiplierAsync(request.MunicipalityId);

        decimal premium = basePremium * engineVolumeMultiplier * ageMultiplier * municipalityMultiplier;

        _logger.LogInformation("Selected parameters: engine: {EngineVolumeMultiplier} age: {AgeMultiplier} municipality: {MunicipalityMultiplier}",
            engineVolumeMultiplier,
            ageMultiplier,
            municipalityMultiplier);

        Calculation calculation = new Calculation
        {
            VehicleTariffTypeId = request.VehicleTariffTypeId,
            EngineVolumeMultiplier = engineVolumeMultiplier,
            VehiclePurposeId = request.VehiclePurposeId,
            VehicleTypeId = request.VehicleTypeId,
            OwnerAgeId = request.OwnerAgeId,
            AgeMultiplier = ageMultiplier,
            MunicipalityId = request.MunicipalityId,
            MunicipalityMultiplier = municipalityMultiplier,
            BasePremium = basePremium,
            FinalPremium = premium,
        };

        await _calculationRepository.AddCalculationAsync(calculation);
        await _calculationRepository.SaveChangesAsync();

        CalculationResponse calculationResponse = _mapper.Map<CalculationResponse>(calculation);
        return calculationResponse;
    }

    public async Task<CalculationResponse> GetByIdAsync(int id)
    {
        Calculation calculation = await _calculationRepository.GetByIdAsync(id);
        if (calculation is null)
        {
            _logger.LogError("Not results found with this ID: {Id}", id);
            throw new EntityNotFoundException(ExceptionMessages.ResultNotFound);
        }

        CalculationResponse response = _mapper.Map<CalculationResponse>(calculation);
        return response;

    }

    public async Task<PageContainer<CalculationResponse>> GetAllAsync(QueryParameters parameters)
    {
        IList<Calculation> calculations = await _calculationRepository.GetAllAsync(parameters);
        int itemsCount = await _calculationRepository.GetCountAsync(parameters);

        IList<CalculationResponse> response = _mapper.Map<IList<CalculationResponse>>(calculations);

        PageContainer<CalculationResponse> pageContainer =
            new PageContainer<CalculationResponse>(
                response,
                itemsCount,
                parameters.PageSize!.Value,
                parameters.PageNumber!.Value);

        return pageContainer;
    }

    private async Task<decimal> GetEngineVolumeMultiplierAsync(int vehicleTariffTypeId)
    {
        VehicleTariffType? vehicleTariffType = await _tariffTypeRepository.GetByIdAsync(vehicleTariffTypeId);
        if (vehicleTariffType is null)
        {
            _logger.LogError("There was no valid tariff type ID: {Id}", vehicleTariffTypeId);
            throw new EntityNotFoundException(ExceptionMessages.ResultNotFound);
        }

        EngineVolume? volume = await _engineVolumeRepository.GetByIdAsync(vehicleTariffTypeId);
        if (volume is null)
        {
            _logger.LogError("There was no valid engine volume type");
            throw new EntityNotFoundException(ExceptionMessages.ResultNotFound);
        }

        decimal engineVolumeMultiplier = volume.Multiplier;
        _logger.LogInformation("Engine volume information {Description} {Multiplier}", volume.Description, volume.Multiplier);
        return engineVolumeMultiplier;

    }

    private async Task<decimal> GetMunicipalityMultiplierAsync(int municipalityId)
    {
        Municipality? municipality = await _municipalityRepository.GetByIdAsync(municipalityId);
        if (municipality is null)
        {
            _logger.LogError("There was no valid municipality ID: {Id}", municipalityId);
            throw new EntityNotFoundException(ExceptionMessages.ResultNotFound);
        }

        _logger.LogInformation("Municipality information name: {Municipality} - {Multiplier}", municipality.Name, municipality.Multiplier);
        return municipality.Multiplier;
    }

    private async Task<decimal> GetAgeMultiplierAsync(int purposeId, int vehicleTypeId, int? ageGroupId)
    {
        VehiclePurpose? vehiclePurpose = await _purposeRepository.GetByIdAsync(purposeId);

        if (vehiclePurpose is null)
        {
            _logger.LogError("Invalid vehicle purpose id {Id}", purposeId);
            throw new NullReferenceException(ExceptionMessages.NullException);
        }

        decimal ageMultiplier = 1M;
        if (purposeId != 3)
        {
            return ageMultiplier;
        }

        if (vehicleTypeId != 2)
        {
            return ageMultiplier;
        }

        if (!ageGroupId.HasValue)
        {
            _logger.LogError("Owner age group id is NULL value");
            throw new NullReferenceException(ExceptionMessages.NullException);
        }

        AgeGroup? ageGroup = await _ageGroupRepository.GetByIdAsync(ageGroupId.Value);
        if (ageGroup is null)
        {
            _logger.LogError("Owner age group is NULL for age group id {AgeGroupId}", ageGroupId);
            throw new EntityNotFoundException(ExceptionMessages.ResultNotFound);
        }

        ageMultiplier = ageGroup.Multiplier;
        return ageMultiplier;
    }
}