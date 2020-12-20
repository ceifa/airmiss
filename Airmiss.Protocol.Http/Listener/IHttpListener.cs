using Airmiss.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Protocol.Http.Listener
{
    internal interface IHttpListener
    {
        Task Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken);

        Task Stop(CancellationToken cancellationToken);
    }
}
