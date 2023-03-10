using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

public class EngineVolumeRepository : IEngineVolumeRepository
{
    private readonly VehicleContext _dbContext;

    public EngineVolumeRepository(VehicleContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<EngineVolume?> GetByIdAsync(int id)
    {
        EngineVolume? volume = await _dbContext.EngineVolumes
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return volume;
    }
}