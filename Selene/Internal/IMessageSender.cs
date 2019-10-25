using Selene.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Internal
{
    public interface IMessageSender
    {
        Task SendMessageAsync(MessageReceiver messageReceiver, CancellationToken cancellationToken);
    }
}
