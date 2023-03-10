using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

public class PurposeRepository : IPurposeRepository
{
    private readonly VehicleContext _dbContext;

    public PurposeRepository(VehicleContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<VehiclePurpose>> GetAllAsync()
    {
        List<VehiclePurpose> purposes = await _dbContext.VehiclePurposes.ToListAsync();
        return purposes;
    }

    public async Task<VehiclePurpose?> GetByIdAsync(int id)
    {
        VehiclePurpose? purpose = await _dbContext.VehiclePurposes
            .FirstOrDefaultAsync(x => x.Id == id);
        return purpose;
    }
}