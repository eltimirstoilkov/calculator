using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

public class VehicleTypeRepository : IVehicleTypeRepository
{
    private readonly VehicleContext _dbContext;

    public VehicleTypeRepository(VehicleContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IList<VehicleType>> GetAllAsync()
    {
        List<VehicleType> all = await _dbContext.VehicleTypes.ToListAsync();
        return all;
    }

    public Task<VehicleType?> GetByIdAsync(int id)
    {
        Task<VehicleType?> type = _dbContext.VehicleTypes
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return type;
    }

    public Task<bool> IsExistingAsync(string description)
    {
        Task<bool> result = _dbContext.VehicleTypes
            .AnyAsync(x => x.Description == description);
        return result;
    }
}