using System.Threading;
using System.Threading.Tasks;
using Selene.Processor;

namespace Selene.Messaging
{
    public interface IMessageProtocol
    {
        Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}