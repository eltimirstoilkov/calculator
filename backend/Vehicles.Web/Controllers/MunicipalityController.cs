using Business.Interfaces.v1;
using Business.Models.v1.Responses;
using Microsoft.AspNetCore.Mvc;
using Persistence.Util;

namespace Vehicles.Web.Controllers;

[Route("api/municipalities")]
[ApiController]
public class MunicipalityController : ControllerBase
{
    private readonly IMunicipalityService _municipalityService;

    public MunicipalityController(IMunicipalityService municipalityService)
    {
        _municipalityService = municipalityService;
    }

    [HttpGet]
    public async Task<PageContainer<MunicipalityResponse>> GetAllAsync([FromQuery] QueryParameters queryParameters)
    {
        return await _municipalityService.GetAllAsync(queryParameters);
    }

    [HttpGet("{id:int}")]
    public async Task<MunicipalityResponse> GetByIdAsync(int id)
    {
        MunicipalityResponse response = await _municipalityService.GetByIdAsync(id);
        return response;
    }
}