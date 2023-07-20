using System.Runtime.Serialization;
using TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable;

namespace TycoonFactoryScheduler.Domain.Entities.Activities.Exceptions
{
    [Serializable]
    public class MaximumNumberOfWorkersAllowedExceededException : ActivityException
    {
        public MaximumNumberOfWorkersAllowedExceededException(Activity activity) : base(activity) { }

        protected MaximumNumberOfWorkersAllowedExceededException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string MessageForLogging =>
            "The activity type {ActivityType} only allows {MaximumNumberOfWorkersAllowed} workers but it was tried to assign {NumberOfWorkerAssigned}";

        public override List<AutomaticLoggingParameter> AutomaticLoggingParameters =>
            new()
            {
                new AutomaticLoggingParameter("ActivityType", Activity.GetActivityType().ToString()),
                new AutomaticLoggingParameter("MaximumNumberOfWorkersAllowed", Activity.MaximumNumberOfWorkersAllowed.ToString()),
                new AutomaticLoggingParameter("NumberOfWorkerAssigned", Activity.Workers.Count.ToString())
            };
    }
}
