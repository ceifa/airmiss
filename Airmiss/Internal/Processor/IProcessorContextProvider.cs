using Airmiss.Messaging;

namespace Airmiss.Internal.Processor
{
    internal interface IProcessorContextProvider
    {
        ProcessorContext GetProcessorContext(Message message);
    }
}