using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Messaging;

namespace Selene.Internal.Protocol
{
    internal class AggregateProtocol : IMessageProtocol
    {
        private readonly IEnumerable<IMessageProtocolDescriptor> _messageProtocolsDescriptors;

        public AggregateProtocol(IEnumerable<IMessageProtocolDescriptor> messageProtocolsDescriptors)
        {
            _messageProtocolsDescriptors = messageProtocolsDescriptors;
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
            return Task.WhenAll(_messageProtocolsDescriptors.Select(d => d.MessageProtocol).Select(func));
        }
    }
}