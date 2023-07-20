using System.ComponentModel.DataAnnotations;
using TycoonFactoryScheduler.Infrastructure.StaticAdapters;
using TycoonFactoryScheduler.Infrastructure.Validation.Validators;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Workers.GetTopBusy
{
    public class GetTopBusyWorkersRequestDto
    {
        private int? _size;
        private DateTime? _end;
        private DateTime? _start;

        public DateTime Start
        {
            get => _start ?? (_end.HasValue ? End.AddDays(-7) : DateTimeProvider.Now);
            set => _start = value;
        }

        [DateAfterThan(nameof(Start), ErrorMessage = $"{nameof(End)} must be after {nameof(Start)}")]
        public DateTime End
        {
            get => _end ?? Start.AddDays(7);
            set => _end = value;
        }

        [Range(1, int.MaxValue, ErrorMessage = $"{nameof(Size)} must be greater than zero")]
        public int Size
        {
            get => _size ?? 10;
            set => _size = value;
        }
    }
}
