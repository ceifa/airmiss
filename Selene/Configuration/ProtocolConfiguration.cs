using System;
using Selene.Messaging;

namespace Selene.Configuration
{
    public class ProtocolConfiguration
    {
        private readonly Action<IMessageProtocol> _addProtocol;
        private readonly SeleneConfiguration _seleneConfiguration;

        internal ProtocolConfiguration(SeleneConfiguration seleneConfiguration,
            Action<IMessageProtocol> addMessageProtocol)
        {
            _seleneConfiguration = seleneConfiguration ?? throw new ArgumentNullException(nameof(seleneConfiguration));
            _addProtocol = addMessageProtocol ?? throw new ArgumentNullException(nameof(addMessageProtocol));
        }

        public SeleneConfiguration Add(IMessageProtocol messageProtocol)
        {
            if (messageProtocol == null)
                throw new ArgumentNullException(nameof(messageProtocol));

            _addProtocol(messageProtocol);

            return _seleneConfiguration;
        }
    }
}