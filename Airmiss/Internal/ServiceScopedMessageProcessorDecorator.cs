using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Internal.TypeActivator;
using Airmiss.Messaging;
using Airmiss.Processor;
using Microsoft.Extensions.DependencyInjection;

namespace Airmiss.Internal
{
    internal class ServiceScopedMessageProcessorDecorator : IMessageProcessor
    {
        private readonly IClientServiceProvider _clientServiceProvider;
        private readonly IMessageProcessor _messageProcessor;

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
            var scopedMessageProcessor = (IMessageProcessor) scopedServiceProvider
                .ServiceProvider.GetRequiredService(_messageProcessor.GetType());

            return scopedMessageProcessor.ProcessAsync(sender, message, cancellationToken);
        }
    }
}