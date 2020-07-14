using Selene.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Protocol.Http.Listener
{
    internal interface IHttpListener
    {
        Task Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken);

        Task Stop(CancellationToken cancellationToken);
    }
}
