using Newtonsoft.Json;
using NUnit.Framework;

namespace TycoonFactoryScheduler.Test.Helpers
{
    public static class JsonEqualityAsserter
    {
        private static readonly JsonSerializerSettings _defaultSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
        };

        public static void AssertJsonEqualsTo(this object thisObject, object to) => 
            thisObject.AssertJsonEqualsTo(to, _defaultSettings);

        public static void AssertJsonEqualsTo(this object thisObject, object to, JsonSerializerSettings settings)
        {
            var thisObjectJson = JsonConvert.SerializeObject(thisObject, settings);
            var toJson = JsonConvert.SerializeObject(to, settings);

            Assert.That(thisObjectJson, Is.EqualTo(toJson));
        }
    }
}
