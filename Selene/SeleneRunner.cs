using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Messaging;

namespace Selene
{
    public class SeleneRunner
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly IMessageProtocol _messageProtocol;

        public SeleneRunner(IMessageProcessor messageProcessor, IMessageProtocol messageProtocol)
        {
            _messageProcessor = messageProcessor;
            _messageProtocol = messageProtocol;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return _messageProtocol.StartAsync(_messageProcessor, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return _messageProtocol.StopAsync(cancellationToken);
        }
    }
}