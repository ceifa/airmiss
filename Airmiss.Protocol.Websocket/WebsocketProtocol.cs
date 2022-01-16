using Airmiss.Core;
using Airmiss.Messaging;
using Airmiss.Protocol.Websocket.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Protocol.Websocket
{
    internal class WebsocketProtocol : IMessageProtocol, IDisposable
    {
        internal const byte BufferDelimiter = (byte)';';

        private readonly IWebsocketListener _websocketListener;

        public WebsocketProtocol(IWebsocketListener websocketListener)
        {
            _websocketListener = websocketListener;
        }

        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            return _websocketListener.Start(messageProcessor, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _websocketListener.Stop(cancellationToken);
        }

        public void Dispose()
        {
            _websocketListener.Dispose();
        }
    }
}