using System.Threading;
using Airmiss.Core;
using Airmiss.Internal.Processor;
using Airmiss.Messaging;

namespace Airmiss.Internal.Client
{
    internal interface IContextProvider
    {
        IContext GetContext(IClient client, ProcessorContext processorContext, Message message,
            CancellationToken cancellationToken);
    }
}