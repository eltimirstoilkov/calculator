using System.ComponentModel.DataAnnotations;

namespace Business.Models.v1.Requests.CustomValidations
{
    public class NullableNumberValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object inputValue, ValidationContext validationContext)
        {
            if (inputValue is null)
                return ValidationResult.Success;

            int number;
            var ageGroupId = inputValue.ToString();
            var result = int.TryParse(ageGroupId, out number);

            if (!result)
                return new ValidationResult("Age group Id must by number");


            if (number <= 0 || number > int.MaxValue)
                return new ValidationResult("Age group must by in range 1 - 2147483647");


            return ValidationResult.Success;
        }
    }
}
