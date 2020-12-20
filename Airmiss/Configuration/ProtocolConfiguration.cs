using System;
using Airmiss.Internal.Protocol;
using Airmiss.Messaging;

namespace Airmiss.Configuration
{
    public class ProtocolConfiguration
    {
        private readonly Action<IMessageProtocolDescriptor> _addProtocol;
        private readonly AirmissConfiguration _AirmissConfiguration;

        internal ProtocolConfiguration(AirmissConfiguration AirmissConfiguration,
            Action<IMessageProtocolDescriptor> addMessageProtocol)
        {
            _AirmissConfiguration = AirmissConfiguration ?? throw new ArgumentNullException(nameof(AirmissConfiguration));
            _addProtocol = addMessageProtocol ?? throw new ArgumentNullException(nameof(addMessageProtocol));
        }

        public AirmissConfiguration Add(IMessageProtocol messageProtocol)
        {
            if (messageProtocol == null)
                throw new ArgumentNullException(nameof(messageProtocol));

            _addProtocol(new MessageProtocolDescriptor(messageProtocol));

            return _AirmissConfiguration;
        }
    }
}