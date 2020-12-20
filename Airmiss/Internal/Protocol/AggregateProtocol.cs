using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Messaging;

namespace Airmiss.Internal.Protocol
{
    internal class AggregateProtocol : IMessageProtocol, IDisposable
    {
        private readonly IEnumerable<IMessageProtocol> _messageProtocols;

        public AggregateProtocol(IEnumerable<IMessageProtocolDescriptor> messageProtocolsDescriptors)
        {
            _messageProtocols = messageProtocolsDescriptors.Select(d => d.MessageProtocol);
        }

        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken)
        {
            return ExecuteAggregatedAsync(messageProtocol =>
                messageProtocol.SendAsync(receiverIdentity, message, cancellationToken));
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            return ExecuteAggregatedAsync(protocol => protocol.StartAsync(messageProcessor, cancellationToken));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return ExecuteAggregatedAsync(protocol => protocol.StopAsync(cancellationToken));
        }

        private Task ExecuteAggregatedAsync(Func<IMessageProtocol, Task> func)
        {
            return Task.WhenAll(_messageProtocols.Select(func));
        }

        public void Dispose()
        {
            foreach (var disposable in _messageProtocols.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
        }
    }
}