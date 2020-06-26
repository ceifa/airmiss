using System;
using Selene.Internal.Protocol;
using Selene.Messaging;

namespace Selene.Configuration
{
    public class ProtocolConfiguration
    {
        private readonly Action<IMessageProtocolDescriptor> _addProtocol;
        private readonly SeleneConfiguration _seleneConfiguration;

        internal ProtocolConfiguration(SeleneConfiguration seleneConfiguration,
            Action<IMessageProtocolDescriptor> addMessageProtocol)
        {
            _seleneConfiguration = seleneConfiguration ?? throw new ArgumentNullException(nameof(seleneConfiguration));
            _addProtocol = addMessageProtocol ?? throw new ArgumentNullException(nameof(addMessageProtocol));
        }

        public SeleneConfiguration Add(IMessageProtocol messageProtocol)
        {
            if (messageProtocol == null)
                throw new ArgumentNullException(nameof(messageProtocol));

            _addProtocol(new MessageProtocolDescriptor(messageProtocol));

            return _seleneConfiguration;
        }
    }
}