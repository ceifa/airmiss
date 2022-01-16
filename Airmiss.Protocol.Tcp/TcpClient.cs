using Airmiss.Core;
using System;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Protocol.Tcp
{
    internal class TcpClient : IClient
    {
        private readonly NetworkStream _stream;

        public TcpClient(string identity, System.Net.Sockets.TcpClient tcpClient)
        {
            Identity = identity;
            Tcp = tcpClient;

            _stream = tcpClient.GetStream();
        }

        public string Identity { get; }

        public System.Net.Sockets.TcpClient Tcp { get; }

        public Task SendAsync<T>(string correlationId, T content, CancellationToken cancellationToken)
        {
            return SendAsync(correlationId, typeof(T), content, cancellationToken);
        }

        public async Task SendAsync(string correlationId, Type contentType, object content,
            CancellationToken cancellationToken)
        {
            JsonElement? serializedContentBytes = content is null ? null : JsonSerializer.SerializeToElement(content, contentType, DefaultJsonSerializerOptions.Options);
            var response = new MessageResponseContainer
            {
                CorrelationId = correlationId,
                Content = serializedContentBytes
            };
            var serializedResponseBytes = JsonSerializer.SerializeToUtf8Bytes(response, DefaultJsonSerializerOptions.Options);
            await _stream.WriteAsync(serializedResponseBytes, cancellationToken);
        }
    }
}