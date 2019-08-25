using System;
using System.Threading;
using Selene.Processor;

namespace Selene.Protocol.Websocket.Listener
{
    public interface IWebsocketListener : IDisposable
    {
        void Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken);
    }
}