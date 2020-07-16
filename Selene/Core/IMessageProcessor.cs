using System.Threading;
using System.Threading.Tasks;
using Selene.Messaging;
using Selene.Processor;

namespace Selene.Core
{
    public interface IMessageProcessor
    {
        Task<ProcessorResult> ProcessAsync(IClient sender, Message message, CancellationToken cancellationToken);
    }
}