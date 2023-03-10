using Business.Models.v1.Responses;

namespace Business.Interfaces.v1
{
    public interface IVehicleTypeService
    {
        Task<VehicleTypeResponse> GetByIdAsync(int id);

        Task<IList<VehicleTypeResponse>> GetAllAsync();
    }
}
