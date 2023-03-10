using Business.Models.v1.Requests.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.v1.Requests;

public class CalculationRequest
{
    [property: Range(1, int.MaxValue)]
    public int VehicleTariffTypeId { get; set; }

    [property: Range(1, int.MaxValue)]
    public int VehiclePurposeId { get; set; }

    [property: Range(1, int.MaxValue)]
    public int VehicleTypeId { get; set; }

    [property: NullableNumberValidation]
    public int? OwnerAgeId { get; set; }

    [property: Range(1, int.MaxValue)]
    public int MunicipalityId { get; set; }
}