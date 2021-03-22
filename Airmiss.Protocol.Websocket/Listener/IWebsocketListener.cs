using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;

namespace Airmiss.Protocol.Websocket.Listener
{
    internal interface IWebsocketListener
    {
        Task Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken);

        Task Stop(CancellationToken cancellationToken);
    }
}