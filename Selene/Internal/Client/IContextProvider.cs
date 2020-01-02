using Selene.Core;
using Selene.Internal.Processor;
using Selene.Messaging;

namespace Selene.Internal.Client
{
    internal interface IContextProvider
    {
        IContext GetContext(IClient client, ProcessorContext processorContext, Message message);
    }
}
