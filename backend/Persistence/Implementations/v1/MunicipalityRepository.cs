using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Persistence.Util;

namespace Persistence.Implementations.v1;

public class MunicipalityRepository : IMunicipalityRepository
{
    private readonly VehicleContext _dbContext;

    public MunicipalityRepository(VehicleContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Municipality>> GetAllAsync(QueryParameters parameters)
    {
        IQueryable<Municipality> municipalities = _dbContext.Municipalities;

        municipalities = FilterMunicipalities(parameters, municipalities);

        municipalities = parameters.OrderBy switch
        {
            QueryParameters.OrderByName => municipalities.OrderBy(m => m.Name),
            _ => municipalities.OrderBy(m => m.Id)
        };

        municipalities = municipalities
            .Skip(parameters.SkipCount)
            .Take(parameters.PageSize!.Value);

        return await municipalities.ToListAsync();
    }
    
    public async Task<int> GetCountAsync(QueryParameters parameters)
    {
        IQueryable<Municipality> municipalities = _dbContext.Municipalities;

        municipalities = FilterMunicipalities(parameters, municipalities);

        return await municipalities.CountAsync();
    }

    public async Task<Municipality?> GetByIdAsync(int id)
    {
        Municipality? municipality = await _dbContext.Municipalities
            .FirstOrDefaultAsync(x => x.Id == id);

        return municipality;
    }

    public async Task<bool> IsExistingTownAsync(int id)
    {
        return await _dbContext.Municipalities
            .AnyAsync(x => x.Id == id);
    }
    
    private static IQueryable<Municipality> FilterMunicipalities(QueryParameters parameters, IQueryable<Municipality> municipalities)
    {
        if (parameters.Name is not null)
        {
            municipalities = municipalities.Where(m => m.Name.Contains(parameters.Name));
        }
        
        return municipalities;
    }
}