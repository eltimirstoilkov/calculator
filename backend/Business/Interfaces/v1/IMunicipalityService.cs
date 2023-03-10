using Business.Models.v1.Responses;
using Persistence.Util;

namespace Business.Interfaces.v1;

public interface IMunicipalityService
{
    Task<PageContainer<MunicipalityResponse>> GetAllAsync(QueryParameters queryParameters);

    Task<MunicipalityResponse> GetByIdAsync(int id);
}