using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;

namespace Airmiss.Internal.Processor
{
    internal interface IProcessorInvoker
    {
        Task<object?> InvokeAsync(ProcessorContext processorContext, IContext context,
            CancellationToken cancellationToken);
    }
}