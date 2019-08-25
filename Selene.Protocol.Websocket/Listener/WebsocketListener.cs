using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Fleck;
using Selene.Exceptions;
using Selene.Messaging;
using Selene.Processor;
using Selene.Protocol.Websocket.Extensions;

namespace Selene.Protocol.Websocket.Listener
{
    internal class WebsocketListener : IWebsocketListener
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
                    try
                    {
                        var message = data.GetMessage();
                        var result =
                            await messageProcessor.ProcessAsync<object>(webSocketClient.Id, message, cancellationToken);

                        if (result != null)
                            await socket.Send(JsonSerializer.ToString(result));
                    }
                    catch (SeleneException ex)
                    {
                        await socket.Send(ex.SerializeJson());
                    }
                    catch (Exception ex)
                    {
                        await socket.Send(new SeleneException(Reason.GenericError, ex.Message).SerializeJson());
                    }
                };
            });
        }

        public Task Send(string connectionId, object message)
        {
            var connection = _openConnections.SingleOrDefault(c => c.Id.Equals(connectionId));
            return connection?.WebSocketConnection.Send(JsonSerializer.ToString(message));
        }

        public void Dispose()
        {
            _webSocketServer?.Dispose();
        }
    }
}