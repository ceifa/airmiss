using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Selene.Core;

namespace Selene.Internal
{
    internal static class TypeRegister
    {
        public static IServiceProvider RegisterTypesAndGetProvider(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IMessageProcessor, MessageProcessor>();
            serviceCollection.TryAddSingleton<ITypeActivator, CachedTypeActivator>();

            return serviceCollection
                .AddSingleton<SeleneRunner>()
                .BuildServiceProvider(new ServiceProviderOptions
                {
                    ValidateOnBuild = true
                });
        }
    }
}
