using System;
using System.Threading;
using System.Threading.Tasks;
using Selene.Processor;

namespace Selene.Protocol.Websocket.Listener
{
    internal interface IWebsocketListener : IDisposable
    {
        void Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken);

        Task Send(string connectionId, object message);
    }
}