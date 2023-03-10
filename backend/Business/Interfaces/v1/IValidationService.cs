namespace Business.Interfaces.v1;

public interface IValidationService
{
    Task ExecuteValidationAsync(int cityId, int tariffId, int typeId, int purposeId);

    Task ExistingTownAsync(int id);
             
    Task ExistingVehicleTariffAsync(int id);
             
    Task ExistingVehicleTypeAsync(int id);
             
    Task ExistingPurposeTypeAsync(int id);
}