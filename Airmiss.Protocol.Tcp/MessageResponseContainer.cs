using System.Text.Json;

namespace Airmiss.Protocol.Tcp
{
    internal class MessageResponseContainer
    {
        public string CorrelationId { get; set; }

        public JsonElement? Content { get; set; }
    }
}
