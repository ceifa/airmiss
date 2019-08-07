using Selene.Processor;
using System;
using System.Threading;

namespace Selene.Protocol.Websocket.Listener
{
    public interface IWebsocketListener : IDisposable
    {
        void Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken);
    }
}
