using System.ComponentModel.DataAnnotations;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Infrastructure.Validation.Validators;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Activities.Post
{
    public class PostActivityRequestDto
    {
        [MinLength(1)]
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        [ValueInEnum(typeof(ActivityType), ErrorMessage = @$"{nameof(ActivityType)} must have one of the values ""BuildComponent"" or ""BuildMachine""")]
        public string ActivityType { get; set; } = string.Empty;

        public DateTime Start { get; set; }

        [DateAfterThan(nameof(Start), ErrorMessage = $"{nameof(End)} must be after {nameof(Start)}")]
        public DateTime End { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = $"{nameof(Workers)} must contain at least one worker")]
        public List<char> Workers { get; set; } = new List<char>();
    }
}