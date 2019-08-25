using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using Fleck;
using Selene.Processor;
using Selene.Protocol.Websocket.Extensions;

namespace Selene.Protocol.Websocket.Listener
{
    public class WebsocketListener : IWebsocketListener
    {
        private readonly List<WebSocketClient> _openConnections;
        private readonly IWebSocketServer _webSocketServer;

        public WebsocketListener(string address)
        {
            _openConnections = new List<WebSocketClient>();
            _webSocketServer = new WebSocketServer(address);
        }

        public void Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            _webSocketServer.Start(socket =>
            {
                var webSocketClient = new WebSocketClient(socket);

                socket.OnOpen = () => _openConnections.Add(webSocketClient);
                socket.OnClose = () => _openConnections.Remove(webSocketClient);
                socket.OnMessage = async data =>
                {
                    var message = data.GetMessage();
                    var result =
                        await messageProcessor.ProcessAsync<object>(webSocketClient.Id, message, cancellationToken);

                    if (result != null)
                        await socket.Send(JsonSerializer.ToUtf8Bytes(result));
                };
            });
        }

        public void Dispose()
        {
            _webSocketServer?.Dispose();
        }
    }
}