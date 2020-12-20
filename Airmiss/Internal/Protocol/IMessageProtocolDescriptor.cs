using Airmiss.Messaging;

namespace Airmiss.Internal.Protocol
{
    internal interface IMessageProtocolDescriptor
    {
        IMessageProtocol MessageProtocol { get; }
    }
}
