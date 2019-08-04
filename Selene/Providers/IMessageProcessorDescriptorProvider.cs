using Selene.Messaging;

namespace Selene.Providers
{
    internal interface IMessageProcessorDescriptorProvider
    {
        MessageProcessorContext GetDescriptor(string route, Verb verb);
    }
}