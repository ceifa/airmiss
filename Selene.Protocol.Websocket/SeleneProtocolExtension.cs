using System;
using Selene.Configuration;
using Selene.Protocol.Websocket.Listener;

namespace Selene.Protocol.Websocket
{
    public static class SeleneProtocolExtension
    {
        public static SeleneConfiguration WebSocket(
            this MessageProtocolConfiguration messageProtocolConfiguration, string address)
        {
            if (messageProtocolConfiguration == null)
                throw new ArgumentNullException(nameof(messageProtocolConfiguration));

            var websocketListener = new WebsocketListener(address);
            var websocketMessageProtocol = new WebsocketMessageProtocol(websocketListener);

            return messageProtocolConfiguration.Add(websocketMessageProtocol);
        }
    }
}