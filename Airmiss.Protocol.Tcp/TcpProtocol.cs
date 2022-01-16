using System;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Messaging;
using Airmiss.Protocol.Tcp.Listener;

namespace Airmiss.Protocol.Tcp
{
    internal class TcpProtocol : IMessageProtocol, IDisposable
    {
        private readonly ITcpListener _tcpListener;

        public TcpProtocol(ITcpListener tcpListener)
        {
            _tcpListener = tcpListener;
        }

        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            return _tcpListener.Start(messageProcessor, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _tcpListener.Stop(cancellationToken);
        }

        public void Dispose()
        {
            _tcpListener.Dispose();
        }
    }
}