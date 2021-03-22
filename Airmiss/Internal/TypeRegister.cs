using System;
using Airmiss.Core;
using Airmiss.Internal.Client;
using Airmiss.Internal.Middleware;
using Airmiss.Internal.Processor;
using Airmiss.Internal.Processor.Hub;
using Airmiss.Internal.Protocol;
using Airmiss.Internal.TypeActivator;
using Airmiss.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Airmiss.Internal
{
    internal static class TypeRegister
    {
        public static IServiceProvider RegisterTypesAndGetProvider(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IMessageProtocol, AggregateProtocol>();
            serviceCollection.TryAddSingleton<IMiddleware, AggregateMiddleware>();
            serviceCollection.TryAddSingleton<IProcessorInvoker, ProcessorInvoker>();
            serviceCollection.TryAddSingleton<IProcessorHubFactory, ProcessorHubFactory>();
            serviceCollection.TryAddSingleton<IProcessorDescriptorProvider, ProcessorDescriptorProvider>();
            serviceCollection.TryAddSingleton<IProcessorContextProvider, ProcessorContextProvider>();
            serviceCollection.TryAddSingleton<ITypeActivator, CachedTypeActivator>();
            serviceCollection.TryAddSingleton<IMessageProcessor, MessageProcessor>();
            serviceCollection.TryAddSingleton<IContextProvider, ContextProvider>();

            return serviceCollection
                .AddSingleton<AirmissRunner>()
                .BuildServiceProvider(new ServiceProviderOptions
                {
                    ValidateOnBuild = true
                });
        }
    }
}