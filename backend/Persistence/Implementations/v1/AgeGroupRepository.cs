using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

public class AgeGroupRepository : IAgeGroupRepository
{
    private readonly VehicleContext _dbContext;

    public AgeGroupRepository(VehicleContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<AgeGroup>> GetAllAsync()
    {
        List<AgeGroup> ageGroups = await _dbContext.AgeGroups.ToListAsync();
        return ageGroups;
    }

    public async Task<AgeGroup?> GetByIdAsync(int id)
    {
        AgeGroup? ageGroup = await _dbContext.AgeGroups
            .FirstOrDefaultAsync(x => x.Id == id);
        return ageGroup;
    }
}