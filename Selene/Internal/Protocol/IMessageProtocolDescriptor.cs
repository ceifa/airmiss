using Selene.Messaging;

namespace Selene.Internal.Protocol
{
    internal interface IMessageProtocolDescriptor
    {
        IMessageProtocol MessageProtocol { get; }
    }
}
