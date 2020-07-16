﻿using Microsoft.Extensions.DependencyInjection;
using Selene.Core;
using Selene.Internal.TypeActivator;
using Selene.Messaging;
using Selene.Processor;
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

        public Task<ProcessorResult> ProcessAsync(IClient sender, Message message, CancellationToken cancellationToken)
        {
            using var scopedServiceProvider = _clientServiceProvider.ServiceProvider.CreateScope();
            var scopedMessageProcessor = (IMessageProcessor)scopedServiceProvider
                .ServiceProvider.GetRequiredService(_messageProcessor.GetType());

            return scopedMessageProcessor.ProcessAsync(sender, message, cancellationToken);
        }
    }
}
