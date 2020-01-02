using Selene.Core;
using Selene.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Tests.Dummies
{
    public class DummyMessageProtocol : IMessageProtocol
    {
        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
