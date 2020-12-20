using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace Airmiss.Internal.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        public static void TryAddDecorator<TService, TDecorator>(
            this IServiceCollection serviceCollection)
            where TService : class
            where TDecorator : class, TService
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            for (int i = 0; i < serviceCollection.Count; i++)
            {
                if (serviceCollection[i].ServiceType.IsAssignableFrom(typeof(TService)))
                {
                    Func<IServiceProvider, object> implementationFactory = provider => ActivatorUtilities.CreateInstance(
                                provider, typeof(TDecorator), provider.GetRequiredService(typeof(TService)));

                    serviceCollection[i] = new ServiceDescriptor(
                        typeof(TService), implementationFactory, serviceCollection[i].Lifetime);
                }
            }
        }
    }
}
