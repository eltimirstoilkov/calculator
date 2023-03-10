namespace Persistence.Util;

public class Constants
{

    public class DefaultValues
    {
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 20;

        public const string DateFormat = "dd.MM.yyyy";

        public const decimal SmallTownsDefaultMultiplier = 0.85m;
        public const decimal EngineVolumeDefaultMultiplier = 1.1m;
        public const decimal AgeGroupDefaultMultiplier = 1m;
    }
    

    public class ExceptionMessages
    {
        public const string InvalidStartDate = "Invalid start date";
        public const string InvalidEndDate = "Invalid end date";
        public const string InvalidDateRange = "The 'end Date' is before 'start Date";

        public const string NotFoundPage = "Not found page";

        public const string VehicleInfoNotFound = "There was no valid vehicle info";
        public const string MunicipalityNotFound = "There was no valid municipality";
        public const string PurposeNotFound = "There was no valid purpose";
        public const string TariffTypeNotFound = "There was no valid tariff type";
        public const string VehicleTypeNotFound = "There was no valid vehicle type";
        public const string NullException = "Invalid value";
        public const string ResultNotFound = "Not results found";
    }


    public class ValidationMessages
    {
        public const string RequiredErrorMessage = "{0} is required";
        public const string RangeErrorMessage = "{0} must by between {1} - {2}";
        public const string EnumTypeErrorMessage = "Not supported type";
    }
}