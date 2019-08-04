using System;
using Selene.Configuration;

namespace Selene.Protocol.Websocket
{
    public static class SeleneProtocolExtension
    {
        public static MessageProtocolConfiguration WebSocket(
            this MessageProtocolConfiguration messageProtocolConfiguration)
        {
            if (messageProtocolConfiguration == null)
                throw new ArgumentNullException(nameof(messageProtocolConfiguration));

            
            return messageProtocolConfiguration;
        }
    }
}
