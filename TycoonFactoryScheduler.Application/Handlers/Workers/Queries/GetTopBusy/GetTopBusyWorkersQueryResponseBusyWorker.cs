namespace TycoonFactoryScheduler.Application.Handlers.Workers.Queries.GetTopBusy
{
    public class GetTopBusyWorkersQueryResponseBusyWorker
    {
        public char Id { get; set; }
        public TimeSpan TimeBusy { get; set; }
    }
}