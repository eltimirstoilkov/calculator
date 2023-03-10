using Business.Interfaces.v1;
using Business.Models.v1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Vehicles.Web.Controllers;

[Route("api/tariffTypes")]
[ApiController]
public class TariffTypeController : ControllerBase
{
    private readonly ITariffTypeService _tariffTypeService;

    public TariffTypeController(ITariffTypeService tariffTypeService)
    {
        _tariffTypeService = tariffTypeService;
    }

    [HttpGet]
    public async Task<IList<TariffResponse>> GetAllAsync()
    {
        IList<TariffResponse> tariffResponses = await _tariffTypeService.GetAllAsync();
        return tariffResponses;
    }

    [HttpGet("{id:int}")]
    public async Task<TariffResponse> GetByIdAsync(int id)
    {
        TariffResponse response = await _tariffTypeService.GetByIdAsync(id);
        return response;
    }
}