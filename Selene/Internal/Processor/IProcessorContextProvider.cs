using Selene.Messaging;

namespace Selene.Internal.Processor
{
    internal interface IProcessorContextProvider
    {
        ProcessorContext GetProcessorContext(Message message);
    }
}