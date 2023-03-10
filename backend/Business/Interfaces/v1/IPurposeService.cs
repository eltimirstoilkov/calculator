using Business.Models.v1.Responses;

namespace Business.Interfaces.v1;

public interface IPurposeService
{
    Task<IList<PurposeResponse>> GetAllAsync();

    Task<PurposeResponse> GetByIdAsync(int id);
}