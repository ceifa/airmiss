using System;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Internal.Client;
using Selene.Internal.Processor;
using Selene.Messaging;
using Selene.Processor;

namespace Selene.Internal
{
    internal class MessageProcessor : IMessageProcessor
    {
        private readonly IProcessorContextProvider _processorContextProvider;
        private readonly IContextProvider _contextProvider;
        private readonly IProcessorInvoker _processorInvoker;

        public MessageProcessor(
            IProcessorContextProvider processorContextProvider,
            IContextProvider contextProvider,
            IProcessorInvoker processorInvoker)
        {
            _processorContextProvider = processorContextProvider;
            _contextProvider = contextProvider;
            _processorInvoker = processorInvoker;
        }

        public async Task<ProcessorResult> ProcessAsync(IClient sender, Message message, CancellationToken cancellationToken)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            using var processorContext = _processorContextProvider.GetProcessorContext(message);
            var context =_contextProvider.GetContext(sender, processorContext, message, cancellationToken);

            var resultType = processorContext.ProcessorDescriptor.ProcessorMethod.ReturnType;
            var result = await _processorInvoker.InvokeAsync(processorContext, context, cancellationToken);

            while (result is Task<object> awaitableResult)
            {
                result = await awaitableResult;
            }

            if (result is Task awaitable)
            {
                await awaitable;
                return ProcessorResult.Empty;
            }

            return new ProcessorResult(resultType, result);
        }
    }
}