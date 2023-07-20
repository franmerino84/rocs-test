using System.Runtime.Serialization;

namespace TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable
{
    [Serializable]
    public abstract class AutomaticallyLoggableException : Exception, ILoggableAutomatically
    {
        public abstract string MessageForLogging { get; }

        public abstract List<AutomaticLoggingParameter> AutomaticLoggingParameters { get; }

        public override string Message
        {
            get
            {
                var message = MessageForLogging;
                AutomaticLoggingParameters.ForEach(x => message = message.Replace($"{{{x.Name}}}", x.Value));
                return message;
            }
        }

        protected AutomaticallyLoggableException()
        {

        }

        protected AutomaticallyLoggableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
