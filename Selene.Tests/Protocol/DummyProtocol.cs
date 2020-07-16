using Selene.Core;
using Selene.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Tests.Protocol
{
    public class DummyProtocol : IMessageProtocol
    {
        public IMessageProcessor MessageProcessor { get; private set; }

        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            MessageProcessor = messageProcessor;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            MessageProcessor = null;
            return Task.CompletedTask;
        }

        public Task<T> ProcessAsync<T>(Message message)
        {
            return MessageProcessor.ProcessAsync<T>(null, message, default);
        }
    }
}
