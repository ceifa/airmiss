using System;
using System.Text.Json.Serialization;

namespace Selene.Messaging
{
    [Serializable]
    public class Message
    {
        [JsonPropertyName("route")] public string Route { get; set; }

        [JsonPropertyName("verb")] public Verb Verb { get; set; }

        [JsonPropertyName("content")] public object Content { get; set; }
    }
}