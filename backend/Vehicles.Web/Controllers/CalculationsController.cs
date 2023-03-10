using Business.Interfaces.v1;
using Business.Models.v1.Requests;
using Business.Models.v1.Responses;
using Microsoft.AspNetCore.Mvc;
using Persistence.Util;

namespace Vehicles.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CalculationsController : ControllerBase
{
    private readonly ICalculationService _vehicleService;

    public CalculationsController(ICalculationService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpPost]
    public async Task<CalculationResponse> CalculateAsync(CalculationRequest request)
    {
        CalculationResponse response = await _vehicleService.CalculateAsync(request);
        return response;
    }

    [HttpGet]
    public async Task<PageContainer<CalculationResponse>> GetAllAsync([FromQuery] QueryParameters parameters)
    {
        PageContainer<CalculationResponse> responses = await _vehicleService.GetAllAsync(parameters);
        return responses;
    }

    [HttpGet("{id:int}")]
    public async Task<CalculationResponse> GetByIdAsync(int id)
    {
        CalculationResponse response = await _vehicleService.GetByIdAsync(id);
        return response;
    }
}