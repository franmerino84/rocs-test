using TycoonFactoryScheduler.Domain.Entities.Workers;

namespace TycoonFactoryScheduler.Abstractions.Persistence.Workers.Models
{
    public class BusyWorker
    {
        public BusyWorker(Worker worker, TimeSpan timeBusy)
        {
            Worker = worker;
            TimeBusy = timeBusy;
        }

        public Worker Worker { get; set; }
        public TimeSpan TimeBusy { get; set; }
    }
}
