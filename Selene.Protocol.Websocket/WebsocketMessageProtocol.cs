using Selene.Messaging;
using Selene.Processor;
using Selene.Protocol.Websocket.Listener;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Protocol.Websocket
{
    public class WebsocketMessageProtocol : IMessageProtocol
    {
        private readonly IWebsocketListener _websocketListener;

        public WebsocketMessageProtocol(IWebsocketListener websocketListener)
        {
            _websocketListener = websocketListener;
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            _websocketListener.Start(messageProcessor, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _websocketListener.Dispose();
            return Task.CompletedTask;
        }
    }
}
