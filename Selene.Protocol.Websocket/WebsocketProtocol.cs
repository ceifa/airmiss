using Selene.Core;
using Selene.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Protocol.Websocket
{
    public class WebsocketProtocol : IMessageProtocol
    {
        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
