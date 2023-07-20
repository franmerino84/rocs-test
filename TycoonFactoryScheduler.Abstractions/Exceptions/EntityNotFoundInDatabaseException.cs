using System.Runtime.Serialization;
using TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type. - False positive 
#pragma warning disable CS8601 // Possible null reference assignment. - False positive 
#pragma warning disable CS8618 // Not null property not initialized at constructor. - False positive 
#pragma warning disable CS8604 // Possible null reference argument. - False positive

namespace TycoonFactoryScheduler.Abstractions.Exceptions
{
    [Serializable]
    public class EntityNotFoundInDatabaseException : AutomaticallyLoggableException
    {
        public EntityNotFoundInDatabaseException(Type entityType, object entityId) : base()
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        protected EntityNotFoundInDatabaseException(SerializationInfo info, StreamingContext streamingContext) : base(info, streamingContext)
        {
            EntityType = (Type)info.GetValue(nameof(EntityType), typeof(Type));
            EntityId = info.GetValue(nameof(EntityType), typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(EntityType), EntityType, typeof(Type));
            info.AddValue(nameof(EntityId), EntityId, typeof(object));
            base.GetObjectData(info, context);
        }


        public Type EntityType { get; set; }

        public object EntityId { get; set; }

        public override string MessageForLogging =>
            "The entity of type {EntityType} with id {EntityId} wasn't found in the database";


        public override List<AutomaticLoggingParameter> AutomaticLoggingParameters =>
            new()
            {
                new AutomaticLoggingParameter("EntityType", EntityType.Name),
                new AutomaticLoggingParameter("EntityId", EntityId.ToString()),
            };
    }
}

#pragma warning restore CS8604 // Possible null reference argument. - False positive
#pragma warning restore CS8618 // Not null property not initialized at constructor. - False positive 
#pragma warning restore CS8601 // Possible null reference assignment. - False positive
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type. - False positive