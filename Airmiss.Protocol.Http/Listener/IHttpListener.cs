using System;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;

namespace Airmiss.Protocol.Http.Listener
{
    internal interface IHttpListener : IDisposable
    {
        Task Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken);

        Task Stop(CancellationToken cancellationToken);
    }
}