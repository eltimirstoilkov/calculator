using System.Security.Principal;

namespace Business.Models.v1.Responses;

public class VehicleInfoResponse
{
    //public int EngineCapacity { get; init; }

    //public int VehicleAge { get; init; }

    public string Municipality { get; init; }

    public decimal MunicipalityMultiplier { get; set; }

    public int PurposeId { get; set; }

    public string Purpose { get; init; }

    public int TariffTypeId { get; init; }

    public string TariffType { get; init; }

    public decimal? EngineVolumeMultiplier { get; init; }

    public int? OwnerAge { get; init; }

    public decimal? AgeGroupMultiplier { get; init; }

    public DateTime IssueDate { get; init; }

    public DateTime ValidFrom { get; init; }

    public DateTime ValidTo { get; init; }
}