using System.Text.Json;
using System.Text.Json.Serialization;

namespace Airmiss.Protocol.Tcp
{
    internal static class DefaultJsonSerializerOptions
    {
        static DefaultJsonSerializerOptions()
        {
            Options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            Options.Converters.Add(new JsonStringEnumConverter());
            Options.Converters.Add(new RouteConverter());
        }

        public static JsonSerializerOptions Options { get; }
    }
}
