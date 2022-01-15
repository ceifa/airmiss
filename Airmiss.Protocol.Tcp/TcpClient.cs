using Airmiss.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Protocol.Http
{
    internal class TcpClient : IClient
    {
        public TcpClient(string identity, System.Net.Sockets.TcpClient tcpClient)
        {
            Identity = identity;
            Tcp = tcpClient;
        }

        public string Identity { get; }

        public System.Net.Sockets.TcpClient Tcp { get; }

        public Task SendAsync<T>(string correlationId, T content, CancellationToken cancellationToken)
        {
            return SendAsync(correlationId, typeof(T), content, cancellationToken);
        }

        public Task SendAsync(string correlationId, Type contentType, object content,
            CancellationToken cancellationToken)
        {
        }
    }
}