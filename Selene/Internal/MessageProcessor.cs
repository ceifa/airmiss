using System;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Internal.Client;
using Selene.Internal.Processor;
using Selene.Messaging;

namespace Selene.Internal
{
    internal class MessageProcessor : IMessageProcessor
    {
        private readonly IProcessorContextProvider _processorContextProvider;
        private readonly IContextProvider _contextProvider;
        private readonly IProcessorInvoker _processorInvoker;

        internal MessageProcessor(
            IProcessorContextProvider processorContextProvider,
            IContextProvider contextProvider,
            IProcessorInvoker processorInvoker)
        {
            _processorContextProvider = processorContextProvider;
            _contextProvider = contextProvider;
            _processorInvoker = processorInvoker;
        }

        public async Task<T> ProcessAsync<T>(IClient sender, Message message, CancellationToken cancellationToken)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            using var processorContext = _processorContextProvider.GetProcessorContext(message);
            var context = _contextProvider.GetContext(sender, processorContext, message);
            await _processorInvoker.InvokeAsync(processorContext, context, cancellationToken);
            return default;
        }
    }
}