using Selene.Messaging;

namespace Selene.Internal.Protocol
{
    internal class MessageProtocolDescriptor : IMessageProtocolDescriptor
    {
        public MessageProtocolDescriptor(IMessageProtocol messageProtocol)
        {
            MessageProtocol = messageProtocol;
        }

        public IMessageProtocol MessageProtocol { get; }
    }
}
