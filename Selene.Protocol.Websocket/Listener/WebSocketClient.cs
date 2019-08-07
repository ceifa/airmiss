using Fleck;
using System;

namespace Selene.Protocol.Websocket.Listener
{
    internal class WebSocketClient
    {
        internal string Id { get; }

        internal IWebSocketConnection WebSocketConnection { get; }

        internal WebSocketClient(IWebSocketConnection webSocketConnection)
        {
            Id = Guid.NewGuid().ToString();
            WebSocketConnection = webSocketConnection ?? throw new ArgumentNullException(nameof(webSocketConnection));
        }
    }
}
