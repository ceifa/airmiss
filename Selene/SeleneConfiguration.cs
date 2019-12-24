using System;
using System.Collections.Generic;
using Selene.Configuration;
using Selene.Internal;
using Selene.Messaging;

namespace Selene
{
    public class SeleneConfiguration
    {
        private readonly List<MessageProcessor> _messageProcessorDescriptors =
            new List<MessageProcessor>();

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
            var messageProcessor = new MessageProcessorManager();

            return new SeleneRunner(messageProcessor, aggregateMessageProtocol);
        }
    }
}