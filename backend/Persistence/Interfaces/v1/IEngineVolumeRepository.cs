using Persistence.Entities.v1;

namespace Persistence.Interfaces.v1;

public interface IEngineVolumeRepository
{
    Task<EngineVolume?> GetByIdAsync(int id);
}