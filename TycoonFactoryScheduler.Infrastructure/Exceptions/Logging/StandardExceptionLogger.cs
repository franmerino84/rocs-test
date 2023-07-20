using Microsoft.Extensions.Logging;

namespace TycoonFactoryScheduler.Infrastructure.Exceptions.Logging
{
    public static class StandardExceptionLogger
    {
        public static void LogException<T>(this ILogger<T> logger, Exception ex)
        {
            logger.LogDebug("{Exception}", ex.ToString());
            logger.LogError("{ErrorMessage}", ex.Message);
        }
    }
}
