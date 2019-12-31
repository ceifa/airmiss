using System;

namespace Selene.Internal.Processor.Hub
{
    public class ServiceBasedHubActivator : IHubActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceBasedHubActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetInstance(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return _serviceProvider.GetService(type);
        }

        public void Release(object instance)
        {
        }
    }
}
