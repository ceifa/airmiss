using System;

namespace Selene.Internal.Processor.Hub
{
    public class ServiceBasedTypeActivator : ITypeActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceBasedTypeActivator(IServiceProvider serviceProvider)
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