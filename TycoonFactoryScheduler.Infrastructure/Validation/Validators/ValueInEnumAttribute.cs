using System.ComponentModel.DataAnnotations;

namespace TycoonFactoryScheduler.Infrastructure.Validation.Validators
{
    public class ValueInEnumAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public ValueInEnumAttribute(Type enumType) =>
            _enumType = enumType;

        public override bool IsValid(object? value)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = value as string;

            return Enum.TryParse(_enumType, currentValue, out _);
        }
    }
}
