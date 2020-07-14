using Selene.Core;
using Selene.Messaging;
using Selene.Protocol.Http.Listener;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Protocol.Http
{
    internal class HttpProtocol : IMessageProtocol
    {
        private readonly IHttpListener _httpListener;

        public HttpProtocol(IHttpListener httpListener)
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
