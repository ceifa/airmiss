using Microsoft.Extensions.DependencyInjection;
using Selene.Core;
using Selene.Internal.TypeActivator;
using Selene.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Internal.Processor
{
    internal class ServiceScopedMessageProcessorDecorator : IMessageProcessor
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly IClientServiceProvider _clientServiceProvider;

        public ServiceScopedMessageProcessorDecorator(
            IMessageProcessor messageProcessor,
            IClientServiceProvider clientServiceProvider)
        {
            _messageProcessor = messageProcessor;
            _clientServiceProvider = clientServiceProvider;
        }

        public Task<T> ProcessAsync<T>(IClient sender, Message message, CancellationToken cancellationToken)
        {
            using var scopedServiceProvider = _clientServiceProvider.ServiceProvider.CreateScope();
            var scopedMessageProcessor = (IMessageProcessor)scopedServiceProvider
                .ServiceProvider.GetRequiredService(_messageProcessor.GetType());

            return scopedMessageProcessor.ProcessAsync<T>(sender, message, cancellationToken);
        }
    }
}
