using MediatR;
using TycoonFactoryScheduler.Infrastructure.Validation.Validators;

namespace TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Patch
{
    public class PatchActivityCommand : IRequest<PatchActivityCommandResponse>
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }

        [DateAfterThan(nameof(Start), ErrorMessage = $"{nameof(End)} must be after {nameof(Start)}")]
        public DateTime End { get; set; }
    }
}