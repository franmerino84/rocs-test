using System.Runtime.Serialization;
using TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable;

namespace TycoonFactoryScheduler.Domain.Entities.Activities.Exceptions
{
    [Serializable]
    public class MinimumNumberOfWorkersNeededNotMetException : ActivityException
    {
        public MinimumNumberOfWorkersNeededNotMetException(Activity activity) : base(activity) { }

        protected MinimumNumberOfWorkersNeededNotMetException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string MessageForLogging =>
            "The activity type {ActivityType} needs at least {MinimumNumberOfWorkersNeeded} workers but it was tried to assign {NumberOfWorkerAssigned}";

        public override List<AutomaticLoggingParameter> AutomaticLoggingParameters =>
            new()
            {
                new AutomaticLoggingParameter("ActivityType", Activity.GetActivityType().ToString()),
                new AutomaticLoggingParameter("MinimumNumberOfWorkersNeeded", Activity.MinimumNumberOfWorkersNeeded.ToString()),
                new AutomaticLoggingParameter("NumberOfWorkerAssigned", Activity.Workers.Count.ToString())
            };
    }
}
