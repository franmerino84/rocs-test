using MediatR;
using System.ComponentModel.DataAnnotations;
using TycoonFactoryScheduler.Domain.Entities.Activities;
using TycoonFactoryScheduler.Infrastructure.Validation.Validators;

namespace TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Post
{
    public class PostActivityCommand : IRequest<PostActivityCommandResponse>
    {
        [MinLength(1)]
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        [EnumDataType(typeof(ActivityType))]
        public ActivityType ActivityType { get; set; }

        public DateTime Start { get; set; }

        [DateAfterThan(nameof(Start), ErrorMessage = $"{nameof(End)} must be after {nameof(Start)}")]
        public DateTime End { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = $"{nameof(Workers)} must contain at least one worker")]
        public List<char> Workers { get; set; } = new List<char>();
    }
}