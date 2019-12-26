using Selene.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;

namespace Selene.Internal
{
    internal class AggregateProtocol : IMessageProtocol
    {
        private readonly IEnumerable<IMessageProtocol> _messageProtocols;

        public AggregateProtocol(IEnumerable<IMessageProtocol> messageProtocols)
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
