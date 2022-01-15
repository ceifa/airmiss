using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Messaging;
using Airmiss.Protocol.Http.Listener;

namespace Airmiss.Protocol.Tcp
{
    internal class TcpProtocol : IMessageProtocol
    {
        private readonly ITcpListener _httpListener;

        public TcpProtocol(ITcpListener httpListener)
        {
            _httpListener = httpListener;
        }

        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            return _httpListener.Start(messageProcessor, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _httpListener.Stop(cancellationToken);
        }
    }
}