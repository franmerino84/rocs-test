using System.Runtime.Serialization;

namespace TycoonFactoryScheduler.Application.Exceptions
{
    [Serializable]
    public class UnexpectedApplicationException : Exception
    {

        public UnexpectedApplicationException()
        {
        }

        public UnexpectedApplicationException(string? message) : base(message)
        {
        }

        public UnexpectedApplicationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnexpectedApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}