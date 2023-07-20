using System.ComponentModel.DataAnnotations;

namespace TycoonFactoryScheduler.Infrastructure.Validation
{
    public static class ValidationManager
    {
        public static ICollection<ValidationResult> GetValidationErrors(this object entity)
        {
            var context = new ValidationContext(entity);
            var errors = new List<ValidationResult>();
            Validator.TryValidateObject(entity, context, errors, true);
            return errors;
        }

        public static ICollection<string?> GetValidationErrorsMessages(this object entity)
        {
            var errors = GetValidationErrors(entity);
            return errors.Select(error => error.ErrorMessage).ToList();
        }

        public static bool IsValid(this object entity)
        {
            var context = new ValidationContext(entity);
            var errors = new List<ValidationResult>();
            return Validator.TryValidateObject(entity, context, errors, true);
        }

        public static bool IsNotValid(this object entity) =>
            !entity.IsValid();

        public static void Validate(this object request, string parameterName)
        {
            if (request == null)
                throw new ArgumentNullException(parameterName);

            var errors = request.GetValidationErrors();

            if (errors.Any())
            {
                Exception innerException;

                if (errors.Count == 1)
                    innerException = GetInnerSingleException(errors.First());
                else
                    innerException = GetInnerAggregateException(errors);

                throw new ArgumentException(null, parameterName, innerException);
            }
        }

        private static Exception GetInnerSingleException(ValidationResult validationResult) =>
            new ArgumentException(validationResult.ErrorMessage, validationResult.MemberNames.First());

        private static Exception GetInnerAggregateException(ICollection<ValidationResult> errors) =>
            new AggregateException(errors.Select(GetInnerSingleException));
    }
}
