using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language.Flow;
using NUnit.Framework;


#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace TycoonFactoryScheduler.Test.Helpers
{
    public static class MockLoggerExtensions
    {
        private static void AssertTimes(int count, Times times)
        {
            times.Deconstruct(out int from, out int to);
            Assert.GreaterOrEqual(count, from);
            Assert.LessOrEqual(count, to);
        }

        public static void VerifyLog(this Mock<ILogger> logger, LogLevel level) =>
            Assert.IsNotEmpty(logger.Invocations.Where(x => (LogLevel)x.Arguments[0] == level));

        public static void VerifyLog<TState>(this Mock<ILogger<TState>> logger, LogLevel level) =>
            Assert.IsNotEmpty(logger.Invocations.Where(x => (LogLevel)x.Arguments[0] == level));

        public static void VerifyLog(this Mock<ILogger> logger, LogLevel level, Times times) =>
            AssertTimes(logger.Invocations.Count(x => (LogLevel)x.Arguments[0] == level), times);

        public static void VerifyLog<TState>(this Mock<ILogger<TState>> logger, LogLevel level, Times times) =>
            AssertTimes(logger.Invocations.Count(x => (LogLevel)x.Arguments[0] == level), times);

        public static void VerifyLog(this Mock<ILogger> logger, LogLevel level, string logMessage) =>
            Assert.IsNotEmpty(logger.Invocations.Where(x =>
                (LogLevel)x.Arguments[0] == level && x.Arguments[2].ToString() == logMessage));

        public static void VerifyLog<TState>(this Mock<ILogger<TState>> logger, LogLevel level, string logMessage) =>
            Assert.IsNotEmpty(logger.Invocations.Where(x =>
                (LogLevel)x.Arguments[0] == level && x.Arguments[2].ToString() == logMessage));

        public static void VerifyLogContains(this Mock<ILogger> logger, LogLevel level, string logMessage) =>
            Assert.IsNotEmpty(logger.Invocations.Where(x =>
                (LogLevel)x.Arguments[0] == level && x.Arguments[2].ToString().Contains(logMessage)));

        public static void VerifyLogContains<TState>(this Mock<ILogger<TState>> logger, LogLevel level, string logMessage) =>
            Assert.IsNotEmpty(logger.Invocations.Where(x =>
                (LogLevel)x.Arguments[0] == level && x.Arguments[2].ToString().Contains(logMessage)));

        public static ISetup<ILogger> SetupLog(this Mock<ILogger> logger, LogLevel? logLevel = null)
        {

            return logger.Setup(x => x.Log(
                logLevel == null ? It.IsAny<LogLevel>() : logLevel.Value,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        }

        public static ISetup<ILogger<TState>> SetupLog<TState>(this Mock<ILogger<TState>> logger, LogLevel? logLevel = null)
        {
            return logger.Setup(x => x.Log(
                logLevel == null ? It.IsAny<LogLevel>() : logLevel.Value,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        public static ISetup<ILogger> SetupLog(this Mock<ILogger> logger, string message, LogLevel? logLevel = null)
        {
            return logger.Setup(x => x.Log(
                logLevel == null ? It.IsAny<LogLevel>() : logLevel.Value,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == message),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        public static ISetup<ILogger<TState>> SetupLog<TState>(this Mock<ILogger<TState>> logger, string message, LogLevel? logLevel = null)
        {
            return logger.Setup(x => x.Log(
                logLevel == null ? It.IsAny<LogLevel>() : logLevel.Value,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == message),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "Not real")]
        public static ISetup<ILogger> SetupLog(this Mock<ILogger> logger, LogLevel logLevel,
            EventId eventId, Exception exception, string message) =>
            logger.Setup(x => x.Log(logLevel, eventId, exception, message));

        public static ISetup<ILogger<TState>> SetupLog<TState>(this Mock<ILogger<TState>> logger, LogLevel logLevel,
            EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
            logger.Setup(x => x.Log(logLevel, eventId, state, exception, formatter));

        public static ISetup<ILogger> SetupLogTrace(this Mock<ILogger> logger, string message = null) =>
           message == null ? logger.SetupLog(LogLevel.Trace) : logger.SetupLog(message, LogLevel.Trace);

        public static ISetup<ILogger<TState>> SetupLogTrace<TState>(this Mock<ILogger<TState>> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Trace) : logger.SetupLog(message, LogLevel.Trace);

        public static ISetup<ILogger> SetupLogDebug(this Mock<ILogger> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Debug) : logger.SetupLog(message, LogLevel.Debug);

        public static ISetup<ILogger<TState>> SetupLogDebug<TState>(this Mock<ILogger<TState>> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Debug) : logger.SetupLog(message, LogLevel.Debug);

        public static ISetup<ILogger> SetupLogInformation(this Mock<ILogger> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Information) : logger.SetupLog(message, LogLevel.Information);

        public static ISetup<ILogger<TState>> SetupLogInformation<TState>(this Mock<ILogger<TState>> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Information) : logger.SetupLog(message, LogLevel.Information);

        public static ISetup<ILogger> SetupLogWarning(this Mock<ILogger> logger, string message = null) =>
           message == null ? logger.SetupLog(LogLevel.Warning) : logger.SetupLog(message, LogLevel.Warning);

        public static ISetup<ILogger<TState>> SetupLogWarning<TState>(this Mock<ILogger<TState>> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Warning) : logger.SetupLog(message, LogLevel.Warning);

        public static ISetup<ILogger> SetupLogError(this Mock<ILogger> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Error) : logger.SetupLog(message, LogLevel.Error);

        public static ISetup<ILogger<TState>> SetupLogError<TState>(this Mock<ILogger<TState>> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Error) : logger.SetupLog(message, LogLevel.Error);

        public static ISetup<ILogger> SetupLogCritical(this Mock<ILogger> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Critical) : logger.SetupLog(message, LogLevel.Critical);

        public static ISetup<ILogger<TState>> SetupLogCritical<TState>(this Mock<ILogger<TState>> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.Critical) : logger.SetupLog(message, LogLevel.Critical);

        public static ISetup<ILogger> SetupLogNone(this Mock<ILogger> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.None) : logger.SetupLog(message, LogLevel.None);

        public static ISetup<ILogger<TState>> SetupLogNone<TState>(this Mock<ILogger<TState>> logger, string message = null) =>
            message == null ? logger.SetupLog(LogLevel.None) : logger.SetupLog(message, LogLevel.None);


    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.