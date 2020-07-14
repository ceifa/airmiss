using System;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Messaging;

namespace Selene
{
    public class SeleneRunner : IDisposable
    {
        private readonly object _locker = new object();

        private readonly IMessageProcessor _messageProcessor;
        private readonly IMessageProtocol _messageProtocol;

        public SeleneRunner(IMessageProcessor messageProcessor, IMessageProtocol messageProtocol)
        {
            _messageProcessor = messageProcessor;
            _messageProtocol = messageProtocol;
        }

        public bool IsRunning { get; private set; }

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
            if (!IsRunning)
            {
                throw new InvalidOperationException("Runner cannot be stop because it's not running");
            }

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

        public void Dispose()
        {
            if (!IsRunning && _messageProtocol is IDisposable disposableProtocol)
            {
                disposableProtocol.Dispose();
            }
        }
    }
}