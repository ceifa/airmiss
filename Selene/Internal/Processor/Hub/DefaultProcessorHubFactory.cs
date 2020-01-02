using System;
using Selene.Core;

namespace Selene.Internal.Processor.Hub
{
    internal class DefaultProcessorHubFactory : IProcessorHubFactory
    {
        private readonly ITypeActivator _typeActivator;

        public DefaultProcessorHubFactory(ITypeActivator typeActivator)
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