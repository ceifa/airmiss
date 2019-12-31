using Selene.Internal.Processor.Hub;

namespace Selene.Internal.Processor
{
    internal interface IProcessorHubFactory
    {
        HubLifecycle CreateHub(ProcessorDescriptor processorDescriptor);
    }
}