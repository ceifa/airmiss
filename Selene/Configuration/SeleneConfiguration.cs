using System;
using System.Collections.Generic;
using Selene.Internal;
using Selene.Internal.Providers;
using Selene.Messaging;
using Selene.Processor;

namespace Selene.Configuration
{
    public class SeleneConfiguration
    {
        private readonly List<MessageProcessorDescriptor> _messageProcessorDescriptors =
            new List<MessageProcessorDescriptor>();

        private readonly List<IMessageProtocol> _messageProtocols =
            new List<IMessageProtocol>();

        private IServiceProvider _serviceProvider;

        public SeleneConfiguration()
        {
            Processor = new MessageProcessorConfiguration(this, _messageProcessorDescriptors.Add);
            Protocol = new MessageProtocolConfiguration(this, _messageProtocols.Add);
        }

        public MessageProcessorConfiguration Processor { get; internal set; }

        public MessageProtocolConfiguration Protocol { get; internal set; }

        public SeleneConfiguration UsingServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }

        public SeleneRunner GetRunner()
        {
            var aggregateMessageProtocol = new AggregateMessageProtocol(_messageProtocols);

            var descriptorProvider = new MessageProcessorProvider(_messageProcessorDescriptors);
            var typeActivator = new TypeActivatorCache(_serviceProvider);
            var receiverContextManager = new ReceiverContextManager();
            var subscriptionManager = new SubscriptionManager();

            var messageProcessor = new MessageProcessor(descriptorProvider, typeActivator, receiverContextManager, subscriptionManager, aggregateMessageProtocol);

            return new SeleneRunner(messageProcessor, aggregateMessageProtocol);
        }
    }
}