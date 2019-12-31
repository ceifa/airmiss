using Selene.Messaging;

namespace Selene.Internal.Processor
{
    interface IProcessorContextProvider
    {
        ProcessorContext GerProcessorContext(Message message);
    }
}
