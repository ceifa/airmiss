using Selene.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;

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
