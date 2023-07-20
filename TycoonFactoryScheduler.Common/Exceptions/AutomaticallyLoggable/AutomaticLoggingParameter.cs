namespace TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable
{
    public class AutomaticLoggingParameter
    {
        public AutomaticLoggingParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}