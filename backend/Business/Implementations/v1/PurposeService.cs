using AutoMapper;
using Business.Exceptions;
using Business.Interfaces.v1;
using Business.Models.v1.Responses;
using Microsoft.Extensions.Logging;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using static Persistence.Util.Constants;

namespace Business.Implementations.v1;

public class PurposeService : IPurposeService
{
    private readonly IPurposeRepository _purposeRepository;
    private readonly ILogger<PurposeService> _logger;
    private readonly IMapper _mapper;

    public PurposeService(
        IPurposeRepository purposeRepository,
        ILogger<PurposeService> logger,
        IMapper mapper)
    {
        _purposeRepository = purposeRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IList<PurposeResponse>> GetAllAsync()
    {
        IList<VehiclePurpose> purposes = await _purposeRepository.GetAllAsync();
        IList<PurposeResponse> response = _mapper.Map<IList<PurposeResponse>>(purposes);
        return response;
    }

    public async Task<PurposeResponse> GetByIdAsync(int id)
    {
        VehiclePurpose? purpose = await _purposeRepository.GetByIdAsync(id);
        if (purpose is null)
        {
            _logger.LogError("No valid purpose was found with id {PurposeId}", id);
            throw new EntityNotFoundException(ExceptionMessages.PurposeNotFound);
        }

        var response = _mapper.Map<PurposeResponse>(purpose);
        return response;
    }
}