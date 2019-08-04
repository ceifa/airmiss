using Selene.Processor;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Messaging
{
    public interface IMessageProtocol
    {
        Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}