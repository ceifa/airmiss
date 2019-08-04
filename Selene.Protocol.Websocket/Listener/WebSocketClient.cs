using System;
using System.Collections.Generic;
using System.Text;
using Fleck;

namespace Selene.Protocol.Websocket.Listener
{
    internal class WebSocketClient
    {
        private string Id { get; }

        private IWebSocketConnection WebSocketConnection { get; }

        internal WebSocketClient(IWebSocketConnection webSocketConnection)
        {
            WebSocketConnection = webSocketConnection ?? throw new ArgumentNullException(nameof(webSocketConnection));
        }
    }
}
