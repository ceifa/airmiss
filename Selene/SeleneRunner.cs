using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Messaging;

namespace Selene
{
    public class SeleneRunner
    {
        private readonly IMessageProcessorManager _messageProcessorManager;
        private readonly IMessageProtocol _messageProtocol;

        internal SeleneRunner(IMessageProcessorManager messageProcessorManager, IMessageProtocol messageProtocol)
        {
            _messageProcessorManager = messageProcessorManager;
            _messageProtocol = messageProtocol;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return _messageProtocol.StartAsync(_messageProcessorManager, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return _messageProtocol.StopAsync(cancellationToken);
        }
    }
}