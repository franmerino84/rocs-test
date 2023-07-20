using System.Runtime.Serialization;
using TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable;
using TycoonFactoryScheduler.Domain.Entities.Activities;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type. - False positive 
#pragma warning disable CS8601 // Possible null reference assignment. - False positive 
#pragma warning disable CS8618 // Not null property not initialized at constructor. - False positive 

namespace TycoonFactoryScheduler.Domain.Entities.Workers.Exceptions
{
    [Serializable]
    public class ScheduleConflictException : AutomaticallyLoggableException
    {
        public ScheduleConflictException(Worker worker, Activity newActivity, Activity conflictedActivity)
        {
            Worker = worker;
            NewActivity = newActivity;
            ConflictedActivity = conflictedActivity;
        }



        protected ScheduleConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Worker = (Worker)info.GetValue(nameof(Worker), typeof(Worker));
            NewActivity = (Activity)info.GetValue(nameof(NewActivity), typeof(Activity));
            ConflictedActivity = (Activity)info.GetValue(nameof(ConflictedActivity), typeof(Activity));
        }

        public Activity ConflictedActivity { get; }
        public Activity NewActivity { get; }
        public Worker Worker { get; }

        public override string MessageForLogging =>
            "Cannot assign the activity {NewActivityId} to the worker {WorkerId} because it conflicts with the activity {ConflictedActivityId}";

        public override List<AutomaticLoggingParameter> AutomaticLoggingParameters =>
            new()
            {
                new AutomaticLoggingParameter("NewActivityId", NewActivity.Id!=0? NewActivity.Id.ToString(): "you're trying to create"),
                new AutomaticLoggingParameter("WorkerId", Worker.Id.ToString()),
                new AutomaticLoggingParameter("ConflictedActivityId", ConflictedActivity.Id.ToString()),
            };

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(Worker), Worker, typeof(Worker));
            info.AddValue(nameof(NewActivity), NewActivity, typeof(Activity));
            info.AddValue(nameof(ConflictedActivity), ConflictedActivity, typeof(Activity));
            base.GetObjectData(info, context);
        }
    }
}

#pragma warning restore CS8618 // Not null property not initialized at constructor. - False positive 
#pragma warning restore CS8601 // Possible null reference assignment. - False positive
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type. - False positive