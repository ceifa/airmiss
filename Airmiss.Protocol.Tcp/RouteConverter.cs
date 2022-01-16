using Airmiss.Processor;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Airmiss.Protocol.Tcp
{
    internal class RouteConverter : JsonConverter<Route>
    {
        public override void Write(Utf8JsonWriter writer, Route value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

        public override Route Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new Route(reader.GetString());
        }
    }
}
