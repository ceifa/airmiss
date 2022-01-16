using Airmiss.Core;
using Airmiss.Exceptions;
using Airmiss.Messaging;
using Airmiss.Processor;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Protocol.Websocket.Listener
{
    internal class DefaultWebsocketListener : IWebsocketListener
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
                    var webSocketContext = await context.AcceptWebSocketAsync("airmiss");
                    var webSocket = webSocketContext.WebSocket;

                    var client = new WebsocketClient(Guid.NewGuid().ToString(), webSocket);

                    const int maxMessageSize = 1024;
                    var receiveBuffer = new byte[maxMessageSize];

                    while (webSocket.State == WebSocketState.Open)
                    {
                        try
                        {
                            var receiveResult = await webSocket.ReceiveAsync(
                                new ArraySegment<byte>(receiveBuffer, default, maxMessageSize), cancellationToken);

                            switch (receiveResult.MessageType)
                            {
                                case WebSocketMessageType.Close:
                                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty,
                                        cancellationToken);
                                    break;
                                case WebSocketMessageType.Binary:
                                    await webSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType,
                                        "Cannot accept binary frame", cancellationToken);
                                    break;
                                case WebSocketMessageType.Text:
                                    var count = receiveResult.Count;

                                    while (!receiveResult.EndOfMessage)
                                    {
                                        if (count >= maxMessageSize)
                                        {
                                            await webSocket.CloseAsync(WebSocketCloseStatus.MessageTooBig,
                                                $"Maximum message size: {maxMessageSize} bytes.", cancellationToken);
                                            return;
                                        }

                                        receiveResult = await webSocket.ReceiveAsync(
                                            new ArraySegment<byte>(receiveBuffer, count, maxMessageSize - count),
                                            cancellationToken);
                                        count += receiveResult.Count;
                                    }

                                    var message = GetMessage(receiveBuffer, count);
                                    var result =
                                        await messageProcessor.ProcessAsync(client, message, cancellationToken);

                                    await client.SendAsync(message.CorrelationId, result.Type, result.Result,
                                        cancellationToken);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            var airmissException = ex as AirmissException ?? new AirmissException(500, ex.Message);

                            var outputBuffer = new ArraySegment<byte>(airmissException.SerializeUtf8());
                            await webSocket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, cancellationToken);
                        }
                    }

                    webSocket.Dispose();
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Message GetMessage(byte[] buffer, int endOfStream)
        {
            var span = buffer.AsSpan(default, endOfStream);

            int current = default;

            var correlationId = Encoding.UTF8.GetString(GetMessagePart(span, ref current));
            var route = Encoding.UTF8.GetString(GetMessagePart(span, ref current));
            var verb = Enum.Parse<Verb>(Encoding.UTF8.GetString(GetMessagePart(span, ref current)), true);
            var content = span[current..];

            return new Message
            {
                CorrelationId = correlationId,
                Route = route,
                Verb = verb,
                Content = content.Length > 0 ? JsonSerializer.Deserialize<object>(content) : default
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Span<byte> GetMessagePart(Span<byte> message, ref int currentPointer)
        {
            message = message[currentPointer..];

            var idx = message.IndexOf(WebsocketProtocol.BufferDelimiter);
            currentPointer += idx + 1;

            return message[..idx];
        }

        public void Dispose()
        {
            _httpListener.Close();
        }
    }
}