using System;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Messaging;

namespace Airmiss
{
    public sealed class AirmissRunner : IDisposable
    {
        private readonly object _locker = new();

        private readonly IMessageProcessor _messageProcessor;
        private readonly IMessageProtocol _messageProtocol;

        public AirmissRunner(IMessageProcessor messageProcessor, IMessageProtocol messageProtocol)
        {
            _messageProcessor = messageProcessor;
            _messageProtocol = messageProtocol;
        }

        public bool IsRunning { get; private set; }

        public void Dispose()
        {
            if (!IsRunning && _messageProtocol is IDisposable disposableProtocol) disposableProtocol.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            lock (_locker)
            {
                try
                {
                    new ThreadStart(() =>
                            _messageProtocol.StartAsync(_messageProcessor, cancellationToken))
                        .Invoke();

                    return Task.CompletedTask;
                }
                finally
                {
                    IsRunning = true;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (!IsRunning) throw new InvalidOperationException("Runner cannot be stop because it's not running");

            lock (_locker)
            {
                try
                {
                    return _messageProtocol.StopAsync(cancellationToken);
                }
                finally
                {
                    IsRunning = false;
                }
            }
        }
    }
}