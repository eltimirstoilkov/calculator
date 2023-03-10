namespace Persistence.Entities.v1;

public class VehicleInfo
{
    public Guid Id { get; set; }

    public string PolicyNumber { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public int TariffTypeId { get; set; }

    public VehicleTariffType TariffType { get; set; }

    public int VehicleTypeId { get; set; }

    public VehicleType VehicleType { get; set; }

    public int VehiclePurposeId { get; set; }

    public VehiclePurpose VehiclePurpose { get; set; }

    public int? EngineVolumeId { get; set; }

    public EngineVolume? EngineVolume { get; set; }

    public int MunicipalityId { get; set; }

    public Municipality Municipality { get; set; }

    public int? OwnerAge { get; set; }

    public int? AgeGroupId { get; set; }

    public AgeGroup? AgeGroup { get; set; }

    public int? DamageCount { get; set; }

    public decimal? PaidAmount { get; set; }

    public decimal? PendingAmount { get; set; }
}