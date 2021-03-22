using System;
using Microsoft.Extensions.DependencyInjection;

namespace Airmiss.Internal.TypeActivator
{
    internal class ServiceBasedTypeActivator : ITypeActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceBasedTypeActivator(IClientServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider.ServiceProvider;
        }

        public object GetInstance(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return _serviceProvider.GetRequiredService(type);
        }

        public void Release(object instance)
        {
            // Method intentionally left empty.
        }
    }
}