using System.ComponentModel.DataAnnotations;

namespace TycoonFactoryScheduler.Infrastructure.Validation.Validators
{
    public class DateBeforeThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateBeforeThanAttribute(string comparisonProperty) =>
            _comparisonProperty = comparisonProperty;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = value as DateTime?;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty)
                ?? throw new ArgumentException("Property with this name not found");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage, new string[1] { property.Name });

            return ValidationResult.Success;
        }
    }
}