using Airmiss.Core;
using Airmiss.Internal.Processor.Hub;

namespace Airmiss.Internal.Processor
{
    internal interface IProcessorHubFactory
    {
        HubLifecycle CreateHub(IProcessorDescriptor processorDescriptor);
    }
}