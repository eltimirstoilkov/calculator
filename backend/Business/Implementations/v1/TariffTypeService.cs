using AutoMapper;
using Business.Exceptions;
using Business.Interfaces.v1;
using Business.Models.v1.Responses;
using Microsoft.Extensions.Logging;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

using static Persistence.Util.Constants;

namespace Business.Implementations.v1;

public class TariffTypeService : ITariffTypeService
{
    private readonly ITariffTypeRepository _typeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TariffTypeService> _logger;

    public TariffTypeService(
        ITariffTypeRepository typeRepository,
        IMapper mapper,
        ILogger<TariffTypeService> logger)
    {
        _typeRepository = typeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IList<TariffResponse>> GetAllAsync()
    {
        IList<VehicleTariffType> tariffTypes = await _typeRepository.GetAllAsync();
        IList<TariffResponse> response = _mapper.Map<IList<TariffResponse>>(tariffTypes);
        return response;
    }

    public async Task<TariffResponse> GetByIdAsync(int id)
    {
        VehicleTariffType? vehicleTariff = await _typeRepository.GetByIdAsync(id);
        if (vehicleTariff is null)
        {
            _logger.LogError("No valid tariff type was found with id {TariffTypeId}", id);
            throw new EntityNotFoundException(ExceptionMessages.TariffTypeNotFound);
        }

        TariffResponse response = _mapper.Map<TariffResponse>(vehicleTariff);
        return response;
    }
}