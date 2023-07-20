using System.Runtime.Serialization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8601 // Possible null reference assignment.


namespace TycoonFactoryScheduler.Infrastructure.Ioc
{
    [Serializable]
    public class ConstructorWithoutParametersDoesntExistException : Exception
    {
        public Type Type;

        public ConstructorWithoutParametersDoesntExistException(Type type)
        {
            Type = type;
        }

        public ConstructorWithoutParametersDoesntExistException(string? message, Type type) : base(message)
        {
            Type = type;
        }


        public ConstructorWithoutParametersDoesntExistException(string? message, Exception? innerException, Type type) : base(message, innerException)
        {
            Type = type;
        }


        protected ConstructorWithoutParametersDoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Type = (Type)info.GetValue(nameof(Type), typeof(Type));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(Type), Type, typeof(Type));
            base.GetObjectData(info, context);
        }
    }
}

#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
