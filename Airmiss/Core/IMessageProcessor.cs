using System.Threading;
using System.Threading.Tasks;
using Airmiss.Messaging;
using Airmiss.Processor;

namespace Airmiss.Core
{
    public interface IMessageProcessor
    {
        Task<ProcessorResult> ProcessAsync(IClient sender, Message message, CancellationToken cancellationToken);
    }
}