using System.Threading;
using System.Threading.Tasks;
using Selene.Core;

namespace Selene.Internal.Processor
{
    internal interface IProcessorInvoker
    {
        Task<object> InvokeAsync(ProcessorContext processorContext, IContext context, CancellationToken cancellationToken);
    }
}
