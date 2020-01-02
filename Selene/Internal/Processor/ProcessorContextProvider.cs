using Selene.Messaging;

namespace Selene.Internal.Processor
{
    internal class ProcessorContextProvider : IProcessorContextProvider
    {
        private readonly IProcessorDescriptorProvider _processorDescriptorProvider;
        private readonly IProcessorHubFactory _processorHubFactory;

        public ProcessorContextProvider(
            IProcessorHubFactory processorHubFactory,
            IProcessorDescriptorProvider processorDescriptorProvider)
        {
            _processorHubFactory = processorHubFactory;
            _processorDescriptorProvider = processorDescriptorProvider;
        }

        public ProcessorContext GetProcessorContext(Message message)
        {
            var processorDescriptor =
                _processorDescriptorProvider.GetProcessorContext(message.Verb, message.Route, out var pathVariables);
            var processorHub = _processorHubFactory.CreateHub(processorDescriptor);
            return new ProcessorContext(processorHub, processorDescriptor, pathVariables);
        }
    }
}