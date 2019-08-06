using Fleck;
using System;
using System.Collections.Generic;

namespace Selene.Protocol.Websocket.Listener
{
    public class WebsocketListener : IWebsocketListener, IDisposable
    {
        private readonly List<WebSocketClient> _openConnections;
        private readonly IWebSocketServer _webSocketServer;

        public WebsocketListener(string address)
        {
            _openConnections = new List<WebSocketClient>();
            _webSocketServer = new WebSocketServer(address);
        }

        public IDisposable Start()
        {
            _webSocketServer.Start(socket =>
            {
                var webSocketClient = new WebSocketClient(socket);

                socket.OnOpen = () => _openConnections.Add(webSocketClient);
                socket.OnClose = () => _openConnections.Remove(webSocketClient);
            });
            return this;
        }

        public void Dispose()
        {
        }
    }
}