using Selene.Messaging;
using Selene.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Internal
{
    public class AggregateMessageProtocol : IMessageProtocol
    {
        private readonly IEnumerable<IMessageProtocol> _messageProtocols;

        public AggregateMessageProtocol(IEnumerable<IMessageProtocol> messageProtocols)
        {
            _messageProtocols = messageProtocols;
        }

        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken) =>
           ExecuteAggregatedAsync(messageProtocol => messageProtocol.SendAsync(receiverIdentity, message, cancellationToken));

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken) =>
            ExecuteAggregatedAsync(protocol => protocol.StartAsync(messageProcessor, cancellationToken));

        public Task StopAsync(CancellationToken cancellationToken) =>
            ExecuteAggregatedAsync(protocol => protocol.StopAsync(cancellationToken));

        private Task ExecuteAggregatedAsync(Func<IMessageProtocol, Task> func) =>
            Task.WhenAll(_messageProtocols.Select(func));
    }
}
