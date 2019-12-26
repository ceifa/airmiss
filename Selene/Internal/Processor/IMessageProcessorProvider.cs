using Selene.Messaging;

namespace Selene.Internal.Processor
{
    internal interface IMessageProcessorProvider
    {
        MessageProcessorContext GetProcessorContext(Verb verb, Route route);
    }
}
