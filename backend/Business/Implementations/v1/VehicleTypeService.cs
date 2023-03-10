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
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly ILogger<VehicleTypeService> _logger;
        private readonly IMapper _mapper;

        public VehicleTypeService(
            IVehicleTypeRepository vehicleTypeRepository,
            ILogger<VehicleTypeService> logger,
            IMapper mapper)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IList<VehicleTypeResponse>> GetAllAsync()
        {
            IList<VehicleType> vehicleTypes = await _vehicleTypeRepository.GetAllAsync();
            IList<VehicleTypeResponse> responses = _mapper.Map<IList<VehicleTypeResponse>>(vehicleTypes);

            return responses;
        }


        public async Task<VehicleTypeResponse> GetByIdAsync(int id)
        {
            VehicleType? vehicleType = await _vehicleTypeRepository.GetByIdAsync(id);
            if (vehicleType is null)
            {
                _logger.LogError("There was no vehicle type with ID: {id}", id);
                throw new EntityNotFoundException(ExceptionMessages.ResultNotFound);
            }

            VehicleTypeResponse response = _mapper.Map<VehicleTypeResponse>(vehicleType);
            return response;
        }
    }
}
