using System;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Internal.Processor;
using Selene.Messaging;

namespace Selene.Internal
{
    internal class MessageProcessor : IMessageProcessor
    {
        private readonly IProcessorContextProvider _processorContextProvider;
        private readonly IProcessorInvoker _processorInvoker;

        internal MessageProcessor(
            IProcessorContextProvider processorContextProvider,
            IProcessorInvoker processorInvoker)
        {
            _processorContextProvider = processorContextProvider;
            _processorInvoker = processorInvoker;
        }

        public async Task<T> ProcessAsync<T>(/*MessageReceiver receiver, */Message message, CancellationToken cancellationToken)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            using var processorContext = _processorContextProvider.GerProcessorContext(message);
            await _processorInvoker.InvokeAsync(processorContext, null, cancellationToken);
            return default;
        }
    }
}