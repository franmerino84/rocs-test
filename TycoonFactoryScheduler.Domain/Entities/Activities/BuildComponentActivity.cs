using TycoonFactoryScheduler.Domain.Entities.Workers;

namespace TycoonFactoryScheduler.Domain.Entities.Activities
{
    public class BuildComponentActivity : Activity
    {
        public BuildComponentActivity(string description, DateTime start, DateTime end)
            : base(0, description, start, end) { }

        public BuildComponentActivity(string description, DateTime start, DateTime end, ICollection<Worker> workers)
            : base(0, description, start, end, workers) { }

        public BuildComponentActivity(int id, string description, DateTime start, DateTime end)
            : base(id, description, start, end) { }

        public BuildComponentActivity(int id, string description, DateTime start, DateTime end, ICollection<Worker> workers)
            : base(id, description, start, end, workers) { }

        public override ActivityType GetActivityType() =>
            ActivityType;

        public override TimeSpan GetChargeBatteriesDuration() =>
            ChargeBatteriesDuration;

        public static ActivityType ActivityType =>
            ActivityType.BuildComponent;

        public static TimeSpan ChargeBatteriesDuration =>
           TimeSpan.FromHours(2);

        public override int MinimumNumberOfWorkersNeeded =>
            1;

        public override int MaximumNumberOfWorkersAllowed =>
            1;
    }
}
