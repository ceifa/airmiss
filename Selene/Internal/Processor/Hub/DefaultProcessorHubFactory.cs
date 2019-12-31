using System;

namespace Selene.Internal.Processor.Hub
{
    internal class DefaultProcessorHubFactory : IProcessorHubFactory
    {
        private readonly IHubActivator _hubActivator;

        public DefaultProcessorHubFactory(IHubActivator hubActivator)
        {
            _hubActivator = hubActivator;
        }

        public HubLifecycle CreateHub(ProcessorDescriptor processorDescriptor)
        {
            if (processorDescriptor == null)
                throw new ArgumentNullException(nameof(processorDescriptor));

            var instance = _hubActivator.GetInstance(processorDescriptor.HubType);
            return new HubLifecycle(instance, () => _hubActivator.Release(instance));
        }
    }
}
