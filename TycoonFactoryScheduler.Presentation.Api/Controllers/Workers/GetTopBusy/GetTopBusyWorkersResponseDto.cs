namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Workers.GetTopBusy
{
    public class GetTopBusyWorkersResponseDto
    {
        public GetTopBusyWorkersResponseDto()
        {
            Workers = new List<GetTopBusyWorkersResponseDtoBusyWorker>();
        }

        public List<GetTopBusyWorkersResponseDtoBusyWorker> Workers { get; set; }
    }
}
