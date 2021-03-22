using Airmiss.Core;

namespace Airmiss.Internal.Processor.Hub
{
    internal interface IProcessorHubFactory
    {
        HubLifecycle CreateHub(IProcessorDescriptor processorDescriptor);
    }
}