using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Persistence.Util;

namespace Persistence.Implementations.v1
{
    public class CalculationRepository : ICalculationRepository
    {
        private readonly VehicleContext _vehicleContext;

        public CalculationRepository(VehicleContext vehicleContext)
        {
            _vehicleContext = vehicleContext;
        }

        public async Task AddCalculationAsync(Calculation calculation)
        {
            await _vehicleContext.AddAsync(calculation);
        }

        public async Task<Calculation> GetByIdAsync(int id)
        {
            Calculation? calculation = await _vehicleContext.Calculations
                .FirstOrDefaultAsync(x => x.Id == id);
            return calculation;
        }

        public async Task SaveChangesAsync()
        {
            await _vehicleContext.SaveChangesAsync();
        }

        public async Task<IList<Calculation>> GetAllAsync(QueryParameters parameters)
        {
            IQueryable<Calculation> calculations = _vehicleContext.Calculations;
            calculations = FilterCalculations(parameters, calculations);

            calculations = calculations
                .Skip(parameters.SkipCount)
                .Take(parameters.PageSize!.Value);

            return await calculations.ToListAsync();
        }

        public async Task<int> GetCountAsync(QueryParameters parameters)
        {
            IQueryable<Calculation> calculations = _vehicleContext.Calculations;
            calculations = FilterCalculations(parameters, calculations);

            return await calculations.CountAsync();
        }

        private static IQueryable<Calculation> FilterCalculations(QueryParameters parameters, IQueryable<Calculation> calculations)
        {
            if (parameters.MunicipalityId is not null)
            {
                calculations = calculations
                    .Where(x => x.MunicipalityId == parameters.MunicipalityId);
            }

            if (parameters.VehicleTariffTypeId is not null)
            {
                calculations = calculations
                    .Where(x => x.VehicleTariffTypeId == parameters.VehicleTariffTypeId);
            }

            if (parameters.VehiclePurposeId is not null)
            {
                calculations = calculations
                    .Where(x => x.VehiclePurposeId == parameters.VehiclePurposeId);
            }

            if (parameters.VehicleTypeId is not null)
            {
                calculations = calculations
                    .Where(x => x.VehicleTypeId == parameters.VehicleTypeId);
            }

            if (parameters.OrderByFinalPremium is not null)
            {

                if (parameters.OrderByFinalPremium is true)
                {
                    calculations = calculations
                     .OrderBy(x => x.FinalPremium);
                }
                else
                {
                    calculations = calculations
                     .OrderByDescending(x => x.FinalPremium);
                }


            }

            return calculations;
        }
    }
}
