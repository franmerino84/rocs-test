namespace TycoonFactoryScheduler.Infrastructure.StaticAdapters
{
    public static class DateTimeProvider
    {
        private static Func<DateTime> _dateTimeNowFunction = () => DateTime.Now;
        public static DateTime Now => _dateTimeNowFunction();

        public static void SetNow(Func<DateTime> dateTimeNowFunction) => 
            _dateTimeNowFunction = dateTimeNowFunction;
    }
}
