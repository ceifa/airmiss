using Selene.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Processor
{
    public interface IMessageProcessor
    {
        Task<object> ProcessAsync(string route, Verb verb, object content, CancellationToken cancellationToken);
    }
}