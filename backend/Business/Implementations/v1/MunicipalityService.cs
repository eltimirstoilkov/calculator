using AutoMapper;
using Business.Exceptions;
using Business.Interfaces.v1;
using Business.Models.v1.Responses;
using Microsoft.Extensions.Logging;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Persistence.Util;
using static Persistence.Util.Constants;

namespace Business.Implementations.v1;

public class MunicipalityService : IMunicipalityService
{
    private readonly IMunicipalityRepository _municipalityRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<MunicipalityService> _logger;

    public MunicipalityService(
        IMunicipalityRepository municipalityRepository,
        IMapper mapper, ILogger<MunicipalityService> logger)
    {
        _municipalityRepository = municipalityRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PageContainer<MunicipalityResponse>> GetAllAsync(QueryParameters queryParameters)
    {
        IList<Municipality> municipalities = await _municipalityRepository.GetAllAsync(queryParameters);
        int totalCount = await _municipalityRepository.GetCountAsync(queryParameters);
        
        IList<MunicipalityResponse> municipalityResponses = _mapper.Map<IList<MunicipalityResponse>>(municipalities);
        
        return new PageContainer<MunicipalityResponse>(
            municipalityResponses,
            totalCount,
            queryParameters.PageSize!.Value,
            queryParameters.PageNumber!.Value);
    }

    public async Task<MunicipalityResponse> GetByIdAsync(int id)
    {
        Municipality? municipality = await _municipalityRepository.GetByIdAsync(id);
        if (municipality is null)
        {
            _logger.LogError("No valid municipality was found with id {MunicipalityId}", id);
            throw new EntityNotFoundException(ExceptionMessages.MunicipalityNotFound);
        }

        MunicipalityResponse response = _mapper.Map<MunicipalityResponse>(municipality);
        return response;
    }
}