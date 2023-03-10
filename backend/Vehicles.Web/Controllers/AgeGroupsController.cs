using Business.Interfaces.v1;
using Business.Models.v1.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vehicles.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgeGroupsController : ControllerBase
    {
        private readonly IAgeGroupService _ageGroupService;

        public AgeGroupsController(IAgeGroupService ageGroupService)
        {
            _ageGroupService = ageGroupService;
        }

        [HttpGet]
        public async Task<IList<AgeGroupResponse>> GetAllAsync()
        {
            IList<AgeGroupResponse> responses = await _ageGroupService.GetAllAsync();
            return responses;
        }

        [HttpGet("{id:int}")]
        public async Task<AgeGroupResponse?> GetByIdAsync(int id)
        {
            AgeGroupResponse? response = await _ageGroupService.GetByIdAsync(id);
            return response;
        }
    }
}
