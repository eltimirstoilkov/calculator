using Persistence.Implementations.v1;

namespace Persistence.Entities.v1
{
    public class Calculation
    {
        public int Id { get; set; }
        public int VehicleTariffTypeId { get; set; }    
        public decimal EngineVolumeMultiplier { get; set; }
        public int VehiclePurposeId { get; set; }
        public int VehicleTypeId { get; set; }
        public int? OwnerAgeId { get; set; }
        public decimal AgeMultiplier { get; set; }
        public int MunicipalityId { get; set; }
        public decimal MunicipalityMultiplier { get; set; }
        public decimal BasePremium { get; set; }
        public decimal FinalPremium { get; set; }
    }
}
