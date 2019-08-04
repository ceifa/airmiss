using Selene.Messaging;
using Selene.Processor;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Selene
{
    public class SeleneRunner
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly IMessageProtocol[] _messageProtocols;

        internal SeleneRunner(IMessageProcessor messageProcessor, IMessageProtocol[] messageProtocols)
        {
            _messageProcessor = messageProcessor;
            _messageProtocols = messageProtocols;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(_messageProtocols.Select(m => m.StartAsync(_messageProcessor, cancellationToken)));
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(_messageProtocols.Select(m => m.StopAsync(cancellationToken)));
        }
    }
}