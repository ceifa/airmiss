using Selene.Core;
using Selene.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Tests.Protocol
{
    public class DisposableProtocol : IMessageProtocol, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public Task SendAsync<T>(string receiverIdentity, T message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StartAsync(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
