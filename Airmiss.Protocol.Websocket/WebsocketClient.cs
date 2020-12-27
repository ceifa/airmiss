using Airmiss.Core;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Protocol.Websocket
{
    internal class WebsocketClient : ISendableClient
    {
        internal WebsocketClient(string identity, WebSocket webSocket)
        {
            Identity = identity;
            WebSocket = webSocket;
        }

        public string Identity { get; }

        internal WebSocket WebSocket { get; }

        public Task SendAsync<T>(string correlationId, T content, CancellationToken cancellationToken)
        {
            return SendAsync(correlationId, typeof(T), content, cancellationToken);
        }

        public Task SendAsync(string correlationId, Type contentType, object content, CancellationToken cancellationToken)
        {
            var correlationIdBytes = Encoding.UTF8.GetBytes(correlationId);
            var serializedContentBytes = JsonSerializer.SerializeToUtf8Bytes(content, contentType);

            var outputBytes = new byte[correlationIdBytes.Length + serializedContentBytes.Length + 1];
            correlationIdBytes.CopyTo(outputBytes, 0);
            outputBytes[correlationIdBytes.Length] = WebsocketProtocol.BufferDelimiter;
            serializedContentBytes.CopyTo(outputBytes, correlationIdBytes.Length + 1);

            var outputBuffer = new ArraySegment<byte>(outputBytes);
            return WebSocket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}
