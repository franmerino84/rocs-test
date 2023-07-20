using System.ComponentModel.DataAnnotations;
using TycoonFactoryScheduler.Infrastructure.Validation.Validators;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Activities.Patch
{
    public class PatchActivityRequestDto
    {
        [Required]
        public DateTime Start { get; set; }

        [Required]
        [DateAfterThan(nameof(Start), ErrorMessage = $"{nameof(End)} must be after {nameof(Start)}")]
        public DateTime End { get; set; }
    }
}