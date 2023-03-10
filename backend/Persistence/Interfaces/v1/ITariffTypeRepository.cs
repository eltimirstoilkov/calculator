using Persistence.Entities.v1;

namespace Persistence.Interfaces.v1;

public interface ITariffTypeRepository
{
    Task<IList<VehicleTariffType>> GetAllAsync();

    Task<VehicleTariffType?> GetByIdAsync(int id);
}