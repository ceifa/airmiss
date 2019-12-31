using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Internal.Processor;
using Selene.Messaging;
using Selene.Processor;

namespace Selene.Internal
{
    internal class MessageProcessor : IMessageProcessor
    {
        private readonly IProcessorContextProvider _processorContextProvider;

        internal MessageProcessor(
            IProcessorContextProvider processorContextProvider)
        {
            _processorContextProvider = processorContextProvider;
        }

        public async Task<T> ProcessAsync<T>(MessageReceiver receiver, Message message, CancellationToken cancellationToken)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            using var processorContext = _processorContextProvider.GerProcessorContext(message);
        }
    }
}