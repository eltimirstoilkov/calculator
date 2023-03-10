using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

public class TariffTypeRepository : ITariffTypeRepository
{
    private readonly VehicleContext _dbContext;

    public TariffTypeRepository(VehicleContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<VehicleTariffType>> GetAllAsync()
    {
        List<VehicleTariffType> all = await _dbContext.VehicleTariffTypes.ToListAsync();
        return all;
    }

    public async Task<VehicleTariffType?> GetByIdAsync(int id)
    {
        VehicleTariffType? tariff = await _dbContext.VehicleTariffTypes
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return tariff;
    }
}