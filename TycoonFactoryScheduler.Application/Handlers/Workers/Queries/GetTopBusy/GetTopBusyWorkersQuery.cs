using MediatR;
using System.ComponentModel.DataAnnotations;
using TycoonFactoryScheduler.Infrastructure.Validation.Validators;

namespace TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy
{
    public class GetTopBusyWorkersQuery : IRequest<GetTopBusyWorkersQueryResponse>
    {
        public DateTime Start { get; set; }

        [DateAfterThan(nameof(Start), ErrorMessage = $"{nameof(End)} must be after {nameof(Start)}")]
        public DateTime End { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = $"{nameof(Size)} must be greater than zero")]
        public uint Size { get; set; }
    }
}
