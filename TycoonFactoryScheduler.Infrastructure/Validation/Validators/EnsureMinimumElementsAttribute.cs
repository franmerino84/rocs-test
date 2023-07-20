using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace TycoonFactoryScheduler.Infrastructure.Validation.Validators
{
    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minimumElements;
        public EnsureMinimumElementsAttribute(int minimumElements)
        {
            _minimumElements = minimumElements;
        }
        public override bool IsValid(object? value)
        {
            if (value is IList list)
                return list.Count >= _minimumElements;

            return false;
        }
    }
}
