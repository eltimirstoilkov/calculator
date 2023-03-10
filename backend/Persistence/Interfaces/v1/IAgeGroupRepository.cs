using Persistence.Entities.v1;

namespace Persistence.Interfaces.v1;

public interface IAgeGroupRepository
{
    Task<IList<AgeGroup>> GetAllAsync();

    Task<AgeGroup?> GetByIdAsync(int id);
}