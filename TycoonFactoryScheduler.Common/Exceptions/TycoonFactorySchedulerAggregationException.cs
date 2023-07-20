using System.Runtime.Serialization;

namespace TycoonFactoryScheduler.Common.Exceptions
{
    [Serializable]
    public class TycoonFactorySchedulerAggregationException : AggregateException
    {
        public IEnumerable<string> Messages =>
            InnerExceptions.Select(x => x.Message);

        public TycoonFactorySchedulerAggregationException() : base() { }

        public TycoonFactorySchedulerAggregationException(string? message)
            : base(message) { }

        public TycoonFactorySchedulerAggregationException(string? message, Exception innerException)
            : base(message, innerException) { }

        protected TycoonFactorySchedulerAggregationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }

        public TycoonFactorySchedulerAggregationException(IEnumerable<Exception> innerExceptions) : base(innerExceptions) { }

        public TycoonFactorySchedulerAggregationException(params Exception[] innerExceptions) : base(innerExceptions) { }

        public TycoonFactorySchedulerAggregationException(string? message, IEnumerable<Exception> innerExceptions) : base(message, innerExceptions) { }

        public TycoonFactorySchedulerAggregationException(string? message, params Exception[] innerExceptions) : base(message, innerExceptions) { }
    }
}
