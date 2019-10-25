using Selene.Messaging;

namespace Selene.Internal.Providers
{
    internal interface IMessageProcessorProvider
    {
        MessageProcessorContext GetProcessor(Route route, Verb verb);
    }
}