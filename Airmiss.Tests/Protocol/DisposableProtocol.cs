using System;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Messaging;

namespace Airmiss.Tests.Protocol
{
    public sealed class DisposableProtocol : IMessageProtocol, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }

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
    }
}