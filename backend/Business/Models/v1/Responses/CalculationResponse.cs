namespace Business.Models.v1.Responses
{
    public record CalculationResponse
        (int Id,
        int VehicleTariffTypeId,
        decimal EngineVolumeMultiplier,
        int VehiclePurposeId,
        int VehicleTypeId,
        int? OwnerAgeId,
        decimal AgeMultiplier,
        int MunicipalityId,
        decimal MunicipalityMultiplier,
        decimal BasePremium,
        decimal FinalPremium);
}