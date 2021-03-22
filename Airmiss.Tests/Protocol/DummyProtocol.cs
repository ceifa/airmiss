using System;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Messaging;

namespace Airmiss.Tests.Protocol
{
    public class DummyProtocol : IMessageProtocol
    {
        public IMessageProcessor MessageProcessor { get; private set; }

        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken)
        {
#pragma warning disable RCS1079 // Throwing of new NotImplementedException.
            throw new NotImplementedException();
#pragma warning restore RCS1079 // Throwing of new NotImplementedException.
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            MessageProcessor = messageProcessor;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            MessageProcessor = null;
            return Task.CompletedTask;
        }

        public async Task<T> ProcessAsync<T>(Message message)
        {
            var result = await MessageProcessor.ProcessAsync(null, message, default);
            return (T) result.Result;
        }
    }
}