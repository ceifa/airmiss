using System.Text.Json.Serialization;
using Selene.Messaging;

namespace Selene.Protocol.Websocket.Extensions
{
    internal static class StringExtensions
    {
        internal static Message GetMessage(this string message)
        {
            return JsonSerializer.Parse<Message>(message);
        }
    }
}