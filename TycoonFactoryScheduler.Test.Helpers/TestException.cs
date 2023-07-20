using System.Runtime.Serialization;

namespace TycoonFactoryScheduler.Test.Helpers
{
    [Serializable]
    public class TestException : Exception
    {
        public TestException()
        {
        }

        public TestException(string? message) : base(message)
        {
        }

        public TestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}