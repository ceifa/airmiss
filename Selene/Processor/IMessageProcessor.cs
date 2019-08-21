using Selene.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Processor
{
    public interface IMessageProcessor
    {
        Task<T> ProcessAsync<T>(string connectionId, Message message, CancellationToken cancellationToken);
    }
}