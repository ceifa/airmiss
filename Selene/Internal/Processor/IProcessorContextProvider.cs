using Selene.Messaging;

namespace Selene.Internal.Processor
{
    internal interface IProcessorContextProvider
    {
        ProcessorContext GerProcessorContext(Message message);
    }
}