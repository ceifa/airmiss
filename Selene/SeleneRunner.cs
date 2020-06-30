using System;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Messaging;

namespace Selene
{
    public class SeleneRunner : IDisposable
    {
        public bool IsRunning { get; private set; }

        private readonly IMessageProcessor _messageProcessor;
        private readonly IMessageProtocol _messageProtocol;

        public SeleneRunner(IMessageProcessor messageProcessor, IMessageProtocol messageProtocol)
        {
            _messageProcessor = messageProcessor;
            _messageProtocol = messageProtocol;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return _messageProtocol.StartAsync(_messageProcessor, cancellationToken);
            }
            finally
            {
                IsRunning = true;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (!IsRunning)
            {
                throw new InvalidOperationException("Runner cannot be stop because it's not running");
            }

            try
            {
                return _messageProtocol.StopAsync(cancellationToken);
            }
            finally
            {
                IsRunning = false;
            }
        }

        public void Dispose()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Runner cannot be disposed while running");
            }

            if (_messageProtocol is IDisposable disposableProtocol)
            {
                disposableProtocol.Dispose();
            }
        }
    }
}