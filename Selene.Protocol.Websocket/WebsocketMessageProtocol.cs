﻿using System.Threading;
using System.Threading.Tasks;
using Selene.Messaging;
using Selene.Processor;
using Selene.Protocol.Websocket.Listener;

namespace Selene.Protocol.Websocket
{
    internal class WebsocketMessageProtocol : IMessageProtocol
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

        public Task SendAsync<T>(string connectionId, T message, CancellationToken cancellationToken)
        {
            return _websocketListener.Send(connectionId, message);
        }
    }
}