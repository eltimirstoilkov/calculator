using AutoMapper;
using Business.Exceptions;
using Business.Interfaces.v1;
using Business.Models.v1.Responses;
using Microsoft.Extensions.Logging;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

using static Persistence.Util.Constants;

namespace Business.Implementations.v1
{
    public class AgeGroupService : IAgeGroupService
    {
        private readonly IAgeGroupRepository _ageGroupRepository;
        private readonly ILogger<AgeGroupService> _logger;
        private readonly IMapper _mapper;

        public AgeGroupService(
            IAgeGroupRepository ageGroupRepository,
            ILogger<AgeGroupService> logger,
            IMapper mapper)
        {
            _ageGroupRepository = ageGroupRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IList<AgeGroupResponse>> GetAllAsync()
        {
            IList<AgeGroup> ageGroups = await _ageGroupRepository.GetAllAsync();
            IList<AgeGroupResponse> responses = _mapper.Map<IList<AgeGroupResponse>>(ageGroups);

            return responses;
        }

        public async Task<AgeGroupResponse?> GetByIdAsync(int id)
        {
            AgeGroup? ageGroup = await _ageGroupRepository.GetByIdAsync(id);
            if (ageGroup is null)
            {
                _logger.LogError("There was no valid age group with ID: {id}", id);
                throw new EntityNotFoundException(ExceptionMessages.ResultNotFound);
            }

            AgeGroupResponse response = _mapper.Map<AgeGroupResponse>(ageGroup);

            return response;
        }
    }
}
