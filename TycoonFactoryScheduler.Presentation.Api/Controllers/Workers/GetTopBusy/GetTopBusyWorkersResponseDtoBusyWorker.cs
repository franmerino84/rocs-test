namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Workers.GetTopBusy
{
    public class GetTopBusyWorkersResponseDtoBusyWorker
    {
        public char Id { get; set; }
        public TimeSpan TimeBusy { get; set; }
    }
}