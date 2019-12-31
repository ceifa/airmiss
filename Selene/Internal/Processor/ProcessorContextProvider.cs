using Selene.Messaging;

namespace Selene.Internal.Processor
{
    internal class ProcessorContextProvider
    {
        private readonly IProcessorHubFactory _processorHubFactory;
        private readonly IProcessorDescriptorProvider _processorDescriptorProvider;

        public ProcessorContextProvider(
            IProcessorHubFactory processorHubFactory,
            IProcessorDescriptorProvider processorDescriptorProvider)
        {
            _processorHubFactory = processorHubFactory;
            _processorDescriptorProvider = processorDescriptorProvider;
        }

        public ProcessorContext GerProcessorContext(Message message)
        {
            var processorDescriptor =
                _processorDescriptorProvider.GetProcessorContext(message.Verb, message.Route, out var pathVariables);
            var processorHub = _processorHubFactory.CreateHub(processorDescriptor);
            return new ProcessorContext(processorHub, processorDescriptor.ProcessorMethod, pathVariables);
        }
    }
}
