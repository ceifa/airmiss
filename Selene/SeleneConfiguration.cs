using System;
using System.Collections.Generic;
using Selene.Configuration;
using Selene.Core;
using Selene.Internal;
using Selene.Messaging;

namespace Selene
{
    public class SeleneConfiguration
    {
        private readonly List<IMessageProtocol> _messageProtocols = new List<IMessageProtocol>();

        private readonly List<IMiddleware> _middlewares = new List<IMiddleware>();
        private readonly List<ProcessorDescriptor> _processorDescriptors = new List<ProcessorDescriptor>();

        private IServiceProvider _serviceProvider;

        public SeleneConfiguration()
        {
            Processor = new ProcessorConfiguration(this, _processorDescriptors.Add);
            Protocol = new ProtocolConfiguration(this, _messageProtocols.Add);
            Middleware = new MiddlewareConfiguration(this, _middlewares.Add);
        }

        public ProcessorConfiguration Processor { get; internal set; }

        public ProtocolConfiguration Protocol { get; internal set; }

        public MiddlewareConfiguration Middleware { get; internal set; }

        public SeleneConfiguration UsingServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }

        public SeleneRunner GetRunner()
        {
            var aggregateProtocol = new AggregateProtocol(_messageProtocols);
            var messageProcessor = new MessageProcessor();

            return new SeleneRunner(messageProcessor, aggregateMessageProtocol);
        }
    }
}