using Selene.Messaging;
using System.Text.Json;

namespace Selene.Protocol.Websocket.Extensions
{
    internal static class StringExtensions
    {
        internal static Message GetMessage(this string message)
        {
            return JsonSerializer.Deserialize<Message>(message);
        }
    }
}
