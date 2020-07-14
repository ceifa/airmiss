using Selene.Core;
using Selene.Internal.Processor;
using Selene.Messaging;
using System.Threading;

namespace Selene.Internal.Client
{
    internal interface IContextProvider
    {
        IContext GetContext(IClient client, ProcessorContext processorContext, Message message, CancellationToken cancellationToken);
    }
}
