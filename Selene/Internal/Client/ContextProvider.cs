using Selene.Core;
using Selene.Internal.Processor;
using Selene.Messaging;

namespace Selene.Internal.Client
{
    internal class ContextProvider : IContextProvider
    {
        public IContext GetContext(IClient client, ProcessorContext processorContext, Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}
