using Persistence.Entities.v1;
using System.Threading.Tasks;
using Persistence.Util;

namespace Persistence.Interfaces.v1;

public interface IMunicipalityRepository
{

    Task<IList<Municipality>> GetAllAsync(QueryParameters parameters);

    Task<int> GetCountAsync(QueryParameters parameters);

    Task<Municipality?> GetByIdAsync(int id);

    Task<bool> IsExistingTownAsync(int id);
}