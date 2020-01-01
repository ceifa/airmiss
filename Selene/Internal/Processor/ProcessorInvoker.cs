﻿using System.Threading;
using System.Threading.Tasks;
using Selene.Core;

namespace Selene.Internal.Processor
{
    internal class ProcessorInvoker : IProcessorInvoker
    {
        private readonly IMiddleware _middleware;

        public ProcessorInvoker(IMiddleware middleware)
        {
            _middleware = middleware;
        }

        public Task<object> InvokeAsync(ProcessorContext processorContext, IContext context, CancellationToken cancellationToken)
        {
            Task<object> ProcessorInvoke() => Task.FromResult(
                processorContext.ProcessorMethod.Invoke(processorContext.HubInstance, context.Arguments));

            return _middleware.InvokeAsync(context, ProcessorInvoke, cancellationToken);
        }
    }
}
