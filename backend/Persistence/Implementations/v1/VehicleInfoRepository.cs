using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Interfaces.v1;

namespace Persistence.Implementations.v1;

public class VehicleInfoRepository : IVehicleInfoRepository
{
    private readonly VehicleContext _dbContext;

    public VehicleInfoRepository(VehicleContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<decimal> GetTotalPendingAmountAsync()
    {
         decimal? pendingAmount = await _dbContext.VehicleInfos.SumAsync(vi => vi.PendingAmount);
         return pendingAmount ?? 0.0M;
    }
    
    public async Task<decimal> GetTotalPaidAmountAsync()
    {
        decimal? paidAmount = await _dbContext.VehicleInfos.SumAsync(vi => vi.PaidAmount);
        return paidAmount ?? 0.0M;
    }
    
    public async Task<int> GetPolicyCountAsync()
    {
        int count = await _dbContext.VehicleInfos.CountAsync();
        return count;
    }
}