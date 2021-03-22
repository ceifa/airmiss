using System;
using Airmiss.Core;
using Airmiss.Internal.TypeActivator;

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
            if (instance is null)
            {
                throw new TypeLoadException($"Could not get instance for type '{processorDescriptor.HubType.FullName}'");
            }

            return new HubLifecycle(instance, () => _typeActivator.Release(instance));
        }
    }
}