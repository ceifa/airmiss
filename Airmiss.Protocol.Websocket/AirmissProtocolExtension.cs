using System;
using Airmiss.Configuration;
using Airmiss.Protocol.Websocket.Listener;

namespace Airmiss.Protocol.Websocket
{
    public static class AirmissProtocolExtension
    {
        public static AirmissConfiguration Websocket(
            this ProtocolConfiguration protocolConfiguration,
            params string[] addresses)
        {
            if (protocolConfiguration is null) throw new ArgumentNullException(nameof(protocolConfiguration));

            var httpListener = new DefaultWebsocketListener(addresses);
            var websocketProtocol = new WebsocketProtocol(httpListener);

            return protocolConfiguration.Add(websocketProtocol);
        }
    }
}