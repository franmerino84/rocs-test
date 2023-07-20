using System.Runtime.Serialization;
using TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type. - False positive 
#pragma warning disable CS8601 // Possible null reference assignment. - False positive 
#pragma warning disable CS8618 // Not null property not initialized at constructor. - False positive 


namespace TycoonFactoryScheduler.Domain.Entities.Activities.Exceptions
{
    [Serializable]
    public abstract class ActivityException : AutomaticallyLoggableException
    {
        public Activity Activity { get; private set; }

        protected ActivityException(Activity activity)
        {
            Activity = activity;
        }

        protected ActivityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Activity = (Activity)info.GetValue(nameof(Activity), typeof(Activity));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(Activity), Activity, typeof(Activity));
            base.GetObjectData(info, context);
        }
    }
}

#pragma warning restore CS8618 // Not null property not initialized at constructor. - False positive 
#pragma warning restore CS8601 // Possible null reference assignment. - False positive
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type. - False positive