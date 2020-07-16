using Selene.Core;
using Selene.Exceptions;
using Selene.Messaging;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Protocol.Websocket.Listener
{
    public class DefaultWebsocketListener : IWebsocketListener
    {
        private readonly HttpListener _httpListener;

        public DefaultWebsocketListener(string[] addresses)
        {
            _httpListener = new HttpListener();

            foreach (var address in addresses)
                _httpListener.Prefixes.Add(address);
        }

        public async Task Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            _httpListener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var context = await _httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                    WebSocket webSocket = webSocketContext.WebSocket;

                    var client = new WebsocketClient(Guid.NewGuid().ToString());

                    const int maxMessageSize = 1024;
                    byte[] receiveBuffer = new byte[maxMessageSize];

                    while (webSocket.State == WebSocketState.Open)
                    {
                        try
                        {
                            WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellationToken);

                            if (receiveResult.MessageType == WebSocketMessageType.Close)
                            {
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
                            }
                            else if (receiveResult.MessageType == WebSocketMessageType.Binary)
                            {
                                await webSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "Cannot accept binary frame", cancellationToken);
                            }
                            else
                            {
                                int count = receiveResult.Count;

                                while (!receiveResult.EndOfMessage)
                                {
                                    if (count >= maxMessageSize)
                                    {
                                        await webSocket.CloseAsync(WebSocketCloseStatus.MessageTooBig, $"Maximum message size: {maxMessageSize} bytes.", cancellationToken);
                                        return;
                                    }

                                    receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer, count, maxMessageSize - count), cancellationToken);
                                    count += receiveResult.Count;
                                }

                                var message = GetMessage(receiveBuffer);
                                var result = await messageProcessor.ProcessAsync(client, message, cancellationToken);

                                var outputBytes = JsonSerializer.SerializeToUtf8Bytes(result.Result, result.Type, default);
                                var outputBuffer = new ArraySegment<byte>(outputBytes);

                                await webSocket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, cancellationToken);
                            }
                        }
                        catch (Exception ex)
                        {
                            var seleneException = ex as SeleneException ?? new SeleneException(500, ex.Message);

                            var outputBuffer = new ArraySegment<byte>(seleneException.SerializeUtf8());
                            await webSocket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, cancellationToken);
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        public Task Stop(CancellationToken cancellationToken)
        {
            _httpListener.Stop();
            return Task.CompletedTask;
        }

        private Message GetMessage(byte[] buffer)
        {
            return JsonSerializer.Deserialize<Message>(buffer);
        }
    }
}
