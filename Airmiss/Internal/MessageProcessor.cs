using System;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Internal.Client;
using Airmiss.Internal.Processor;
using Airmiss.Messaging;
using Airmiss.Processor;

namespace Airmiss.Internal
{
    internal class MessageProcessor : IMessageProcessor
    {
        private readonly IContextProvider _contextProvider;
        private readonly IProcessorContextProvider _processorContextProvider;
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

        public async Task<ProcessorResult> ProcessAsync(IClient sender, Message message,
            CancellationToken cancellationToken)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            using var processorContext = _processorContextProvider.GetProcessorContext(message);
            var context = _contextProvider.GetContext(sender, processorContext, message, cancellationToken);

            var resultType = processorContext.ProcessorDescriptor.ProcessorMethod.ReturnType;
            var result = await _processorInvoker.InvokeAsync(processorContext, context, cancellationToken);

            if (result is Task awaitable)
            {
                await awaitable;

                var taskType = result.GetType();

                if (!taskType.IsGenericType) return ProcessorResult.Empty;

                resultType = taskType.GetGenericArguments()[0];
                result = taskType.GetProperty(nameof(Task<object?>.Result))?.GetValue(awaitable);
            }

            return new ProcessorResult(resultType, result);
        }
    }
}