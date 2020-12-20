using System;
using Airmiss.Core;

namespace Airmiss.Internal.Processor.Hub
{
    internal class ProcessorHubFactory : IProcessorHubFactory
    {
        private readonly ITypeActivator _typeActivator;

        public ProcessorHubFactory(ITypeActivator typeActivator)
        {
            _typeActivator = typeActivator;
        }

        public HubLifecycle CreateHub(IProcessorDescriptor processorDescriptor)
        {
            if (processorDescriptor == null)
                throw new ArgumentNullException(nameof(processorDescriptor));

            var instance = _typeActivator.GetInstance(processorDescriptor.HubType);
            return new HubLifecycle(instance, () => _typeActivator.Release(instance));
        }
    }
}