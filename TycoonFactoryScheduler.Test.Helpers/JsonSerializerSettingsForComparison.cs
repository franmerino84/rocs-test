using Newtonsoft.Json;

namespace TycoonFactoryScheduler.Test.Helpers
{
    public class JsonSerializerSettingsForComparison : JsonSerializerSettings
    {
        private static JsonSerializerSettings? _jsonSerializerSettings;
        public static JsonSerializerSettings Instance
        {
            get
            {
                _jsonSerializerSettings ??= new JsonSerializerSettingsForComparison();
                return _jsonSerializerSettings;
            }
        }

        public JsonSerializerSettingsForComparison()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
        }
    }
}
