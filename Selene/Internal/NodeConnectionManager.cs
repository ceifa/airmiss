using System;
using System.Collections.Generic;
using Selene.Messaging;

namespace Selene.Internal.Providers
{
    internal class ReceiverContextManager : IReceiverContextManager
    {
        private readonly IDictionary<string, ReceiverContext> _connectedReceivers;

        public ReceiverContextManager()
        {
            _connectedReceivers = new Dictionary<string, ReceiverContext>();
        }

        public ReceiverContext GetContext(MessageReceiver receiver)
        {
            if (string.IsNullOrWhiteSpace(receiver.Identity))
                throw new ArgumentException("Receiver identity cannot be null or white space", nameof(receiver));

            return _connectedReceivers.TryGetValue(receiver.Identity, out var connectionContext)
                ? connectionContext
                : new ReceiverContext(receiver);
        }

        public void SetContext(ReceiverContext receiverContext)
        {
            if (string.IsNullOrWhiteSpace(receiverContext?.MessageReceiver?.Identity))
                throw new ArgumentException("Receiver or receiver identity cannot be null or white space", nameof(receiverContext));

            _connectedReceivers[receiverContext.MessageReceiver.Identity] = receiverContext;
        }

        public void ReleaseContext(MessageReceiver receiver)
        {
            _connectedReceivers.Remove(receiver.Identity);
        }
    }
}