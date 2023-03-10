using Persistence.Entities.v1;
using Persistence.Util;

namespace Persistence.Interfaces.v1
{
    public interface ICalculationRepository
    {
        Task AddCalculationAsync(Calculation calculation);

        Task<Calculation> GetByIdAsync(int id);

        Task SaveChangesAsync();

        Task<IList<Calculation>> GetAllAsync(QueryParameters parameters);

        Task<int> GetCountAsync(QueryParameters parameters);
    }
}
