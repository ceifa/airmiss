using Selene.Messaging;

namespace Selene.Internal.Providers
{
    internal interface IMessageProcessorDescriptorProvider
    {
        MessageProcessorContext GetDescriptor(Route route, Verb verb);
    }
}