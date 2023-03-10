using Business.Interfaces.v1;
using Business.Models.v1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Vehicles.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypesController : ControllerBase
    {
        private readonly IVehicleTypeService _vehicleTypeService;

        public VehicleTypesController(IVehicleTypeService vehicleTypeService)
        {
            _vehicleTypeService = vehicleTypeService;
        }

        [HttpGet]
        public async Task<IList<VehicleTypeResponse>> GetAllAsync()
        {
            IList<VehicleTypeResponse> responses = await _vehicleTypeService.GetAllAsync();
            return responses;
        }

        [HttpGet("{id:int}")]
        public async Task<VehicleTypeResponse> GetByIdAsync(int id)
        {
            VehicleTypeResponse response = await _vehicleTypeService.GetByIdAsync(id);
            return response;
        }
    }
}
