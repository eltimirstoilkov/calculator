using Business.Models.v1.Responses;

namespace Business.Interfaces.v1
{
    public interface IAgeGroupService
    {
        Task<IList<AgeGroupResponse>> GetAllAsync();

        Task<AgeGroupResponse?> GetByIdAsync(int id);
    }
}
