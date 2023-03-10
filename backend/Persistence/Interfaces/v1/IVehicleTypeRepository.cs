using Persistence.Entities.v1;

namespace Persistence.Interfaces.v1;

public interface IVehicleTypeRepository
{
    public Task<IList<VehicleType>> GetAllAsync();

    public Task<VehicleType?> GetByIdAsync(int id);

    public Task<bool> IsExistingAsync(string description);
}