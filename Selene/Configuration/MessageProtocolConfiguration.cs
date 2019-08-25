using System;
using Selene.Messaging;

namespace Selene.Configuration
{
    public class MessageProtocolConfiguration
    {
        private readonly Action<IMessageProtocol> _addMessageProtocol;
        private readonly SeleneConfiguration _seleneConfiguration;

        internal MessageProtocolConfiguration(SeleneConfiguration seleneConfiguration,
            Action<IMessageProtocol> addMessageProtocol)
        {
            _seleneConfiguration = seleneConfiguration ?? throw new ArgumentNullException(nameof(seleneConfiguration));
            _addMessageProtocol = addMessageProtocol ?? throw new ArgumentNullException(nameof(addMessageProtocol));
        }

        public SeleneConfiguration Add(IMessageProtocol messageProtocol)
        {
            if (messageProtocol == null)
                throw new ArgumentNullException(nameof(messageProtocol));

            _addMessageProtocol(messageProtocol);

            return _seleneConfiguration;
        }
    }
}