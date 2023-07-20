namespace TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy
{
    public class GetTopBusyWorkersQueryResponse
    {
        public GetTopBusyWorkersQueryResponse()
        {
            Workers = new List<GetTopBusyWorkersQueryResponseBusyWorker>();
        }

        public List<GetTopBusyWorkersQueryResponseBusyWorker> Workers { get; set; }
    }
}