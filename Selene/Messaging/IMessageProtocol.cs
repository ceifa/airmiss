using System.Threading;
using System.Threading.Tasks;
using Selene.Core;

namespace Selene.Messaging
{
    public interface IMessageProtocol
    {
        Task StartAsync(IMessageProcessorManager messageProcessorManager, CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);

        Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken);
    }
}