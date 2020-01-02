using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Selene.Internal
{
    internal static class TypeRegister
    {
        public static IServiceProvider RegisterTypesAndGetProvider(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ITypeActivator, CachedTypeActivator>();

            return serviceCollection
                .BuildServiceProvider(new ServiceProviderOptions
                {
                    ValidateOnBuild = true
                });
        }
    }
}
