using TycoonFactoryScheduler.Domain.Entities.Workers;

namespace TycoonFactoryScheduler.Domain.Entities.Activities
{
    public class BuildMachineActivity : Activity
    {
        public BuildMachineActivity(string description, DateTime start, DateTime end)
            : base(0, description, start, end) { }

        public BuildMachineActivity(string description, DateTime start, DateTime end, ICollection<Worker> workers)
            : base(0, description, start, end, workers) { }

        public BuildMachineActivity(int id, string description, DateTime start, DateTime end)
           : base(id, description, start, end) { }

        public BuildMachineActivity(int id, string description, DateTime start, DateTime end, ICollection<Worker> workers)
            : base(id, description, start, end, workers) { }

        public override ActivityType GetActivityType() =>
            ActivityType;

        public override TimeSpan GetChargeBatteriesDuration() =>
            ChargeBatteriesDuration;

        public static ActivityType ActivityType =>
            ActivityType.BuildMachine;

        public static TimeSpan ChargeBatteriesDuration =>
           TimeSpan.FromHours(4);

        public override int MinimumNumberOfWorkersNeeded =>
            1;

        public override int MaximumNumberOfWorkersAllowed =>
            int.MaxValue;
    }
}
