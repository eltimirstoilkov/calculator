using Business.Interfaces.v1;
using Business.Models.v1.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Vehicles.Web.Controllers;

[Route("api/purposes")]
[ApiController]
public class PurposesController : ControllerBase
{
    private readonly IPurposeService _purposeService;

    public PurposesController(IPurposeService purposeService)
    {
        _purposeService = purposeService;
    }

    [HttpGet]
    public async Task<ICollection<PurposeResponse>> GetAllAsync()
    {
        ICollection<PurposeResponse> purposes = await _purposeService.GetAllAsync();
        return purposes;
    }

    [HttpGet("{id:int}")]
    public async Task<PurposeResponse> GetByIdAsync(int id)
    {
        return await _purposeService.GetByIdAsync(id);
    }
}