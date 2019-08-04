using Selene.Messaging;
using Selene.Processor;
using Selene.Providers;
using System;
using System.Collections.Generic;

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
            var descriptorProvider = new MessageProcessorDescriptorProvider(_messageProcessorDescriptors);
            var typeActivator = new TypeActivatorCache(_serviceProvider);

            var messageProcessor = new MessageProcessor(descriptorProvider, typeActivator);

            return new SeleneRunner(messageProcessor, _messageProtocols.ToArray());
        }
    }
}