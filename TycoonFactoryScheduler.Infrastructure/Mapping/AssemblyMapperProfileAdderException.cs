using System.Runtime.Serialization;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type. - False positive 
#pragma warning disable CS8601 // Possible null reference assignment. - False positive 
#pragma warning disable CS8618 // Not null property not initialized at constructor. - False positive 

namespace TycoonFactoryScheduler.Infrastructure.Mapping
{
    [Serializable]
    public class AssemblyMapperProfileAdderException : Exception
    {
        public Type Type { get; private set; }

        public AssemblyMapperProfileAdderException(Type type)
        {
            Type = type;
        }

        protected AssemblyMapperProfileAdderException(SerializationInfo info, StreamingContext context) : base(info, context)
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

#pragma warning restore CS8618 // Not null property not initialized at constructor. - False positive 
#pragma warning restore CS8601 // Possible null reference assignment. - False positive
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type. - False positive