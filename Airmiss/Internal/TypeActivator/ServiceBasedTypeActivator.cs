﻿using Microsoft.Extensions.DependencyInjection;
using Airmiss.Internal.TypeActivator;
using System;

namespace Airmiss.Internal.Processor.Hub
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
        }
    }
}