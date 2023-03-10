using Business.Models.v1.Requests;
using Business.Models.v1.Responses;
using Persistence.Util;

namespace Business.Interfaces.v1;

public interface ICalculationService
{
    Task<CalculationResponse> CalculateAsync(CalculationRequest request);

    Task<CalculationResponse> GetByIdAsync(int id);

    Task<PageContainer<CalculationResponse>> GetAllAsync(QueryParameters parameters);
}