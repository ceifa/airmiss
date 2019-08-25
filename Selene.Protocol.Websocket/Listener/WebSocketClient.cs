using System;
using Fleck;

namespace Selene.Protocol.Websocket.Listener
{
    internal class WebSocketClient
    {
        internal WebSocketClient(IWebSocketConnection webSocketConnection)
        {
            Id = Guid.NewGuid().ToString();
            WebSocketConnection = webSocketConnection ?? throw new ArgumentNullException(nameof(webSocketConnection));
        }

        internal string Id { get; }

        internal IWebSocketConnection WebSocketConnection { get; }
    }
}