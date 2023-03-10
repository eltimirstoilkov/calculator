using Business.Models.v1.Responses;

namespace Business.Interfaces.v1;

public interface ITariffTypeService
{
    Task<IList<TariffResponse>> GetAllAsync();

    Task<TariffResponse> GetByIdAsync(int id);
}