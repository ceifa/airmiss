using Airmiss.Core;

namespace Airmiss.Internal.Client
{
    internal class ClientContext : IContext
    {
        public ClientContext(IClient sender, IProcessorDescriptor processor, object?[] arguments)
        {
            Sender = sender;
            Processor = processor;
            Arguments = arguments;
        }

        public IClient Sender { get; }

        public IProcessorDescriptor Processor { get; }

        public object?[] Arguments { get; }
    }
}
