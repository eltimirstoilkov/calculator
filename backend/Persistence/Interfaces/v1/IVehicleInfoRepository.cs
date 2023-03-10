namespace Persistence.Interfaces.v1;

public interface IVehicleInfoRepository
{
    Task<decimal> GetTotalPendingAmountAsync();

    Task<decimal> GetTotalPaidAmountAsync();

    Task<int> GetPolicyCountAsync();
}