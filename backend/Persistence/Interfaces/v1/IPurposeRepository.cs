using Persistence.Entities.v1;

namespace Persistence.Interfaces.v1;

public interface IPurposeRepository
{
    Task<IList<VehiclePurpose>> GetAllAsync();

    Task<VehiclePurpose?> GetByIdAsync(int id);
}