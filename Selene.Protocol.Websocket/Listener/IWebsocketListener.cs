using System;
using System.Threading;
using Selene.Processor;

namespace Selene.Protocol.Websocket.Listener
{
    internal interface IWebsocketListener : IDisposable
    {
        void Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken);
    }
}