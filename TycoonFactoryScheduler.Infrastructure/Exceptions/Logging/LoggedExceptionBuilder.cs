using Microsoft.Extensions.Logging;
using TycoonFactoryScheduler.Common.Exceptions.AutomaticallyLoggable;

namespace TycoonFactoryScheduler.Infrastructure.Exceptions.Logging
{
    public static class LoggedExceptionBuilder
    {
        public static Exception GetPlainErrorLoggedException<T>(object caller, ILogger<T> logger, Exception innerException, Func<string, Exception, Exception> exceptionCreator) =>
            GetPlainErrorLoggedException(caller.GetType(), logger, innerException, exceptionCreator);

        public static Exception GetPlainErrorLoggedException<T>(Type callerClassType, ILogger<T> logger, Exception innerException, Func<string, Exception, Exception> exceptionCreator) =>
            GetPlainErrorLoggedException(callerClassType.Name, logger, innerException, exceptionCreator);

        public static Exception GetPlainErrorLoggedException<T>(string callerClassName, ILogger<T> logger, Exception innerException, Func<string, Exception, Exception> exceptionCreator) =>
            GetLoggedException(exceptionCreator,
                "Error at {0}",
                new List<AutomaticLoggingParameter> { new AutomaticLoggingParameter("Class", callerClassName) },
                logger,
                innerException,
                LogLevel.Error);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "False positive")]
        public static Exception GetLoggedException<T>(Func<string, Exception, Exception> exceptionCreator,
            string message, List<AutomaticLoggingParameter> parameters, ILogger<T> logger, Exception innerException,
            LogLevel logLevel = LogLevel.Error, bool logDebugFullException = true)
        {
            if (logDebugFullException)
                logger.LogDebug(innerException.ToString());

            var messageForLogging = message.ToString();

            var indexedParametersList = parameters.Select((parameter, index) => new { Index = index, Parameter = parameter })
                .ToList();

            indexedParametersList.ForEach(x => messageForLogging = messageForLogging.Replace($"{{{x.Index}}}", $"{{{x.Parameter.Name}}}"));

            logger.Log(logLevel, messageForLogging, parameters.Select(x => x.Value));

            var messageForException = string.Format(message, parameters.Select(x => x.Value));

            return exceptionCreator(messageForException, innerException);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "Fixed per exception")]
        public static Exception GetLoggedException<T>(Func<AutomaticallyLoggableException, Exception> exceptionCreator,
            ILogger<T> logger, AutomaticallyLoggableException innerException, LogLevel logLevel = LogLevel.Error, bool logDebugFullException = true)
        {
            if (logDebugFullException)
                logger.LogDebug(innerException.ToString());

            logger.Log(logLevel, innerException.MessageForLogging, innerException.AutomaticLoggingParameters.Select(x => x.Value));

            return exceptionCreator(innerException);
        }
    }
}
