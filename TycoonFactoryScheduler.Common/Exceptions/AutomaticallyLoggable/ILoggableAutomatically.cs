namespace TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable
{
    public interface ILoggableAutomatically
    {
        string MessageForLogging { get; }
        List<AutomaticLoggingParameter> AutomaticLoggingParameters { get; }
    }
}
