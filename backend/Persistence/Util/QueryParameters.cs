using static Persistence.Util.Constants;

namespace Persistence.Util;

public class QueryParameters
{
    public const string OrderByName = "name";

    private int? _pageSize;
    private int? _pageNumber;

    public string? Name { get; set; }

    public int? PageSize
    {
        get => _pageSize ?? DefaultValues.DefaultPageSize;
        set
        {
            if (value is null || value <= 0)
            {
                _pageSize = DefaultValues.DefaultPageSize;
            }

            _pageSize = value;
        }
    }

    public int? PageNumber
    {
        get => _pageNumber ?? DefaultValues.DefaultPageNumber;
        set
        {
            if (value is null || value <= 0)
            {
                _pageNumber = DefaultValues.DefaultPageNumber;
            }

            _pageNumber = value;
        }
    }

    public int SkipCount => (PageNumber!.Value - 1) * PageSize!.Value;

    public string? OrderBy { get; set; }

    public bool? OrderByFinalPremium { get; set; }

    public int? VehicleTariffTypeId { get; set; }

    public int? VehiclePurposeId { get; set; }

    public int? MunicipalityId { get; set; }

    public int? VehicleTypeId { get; set; }
}