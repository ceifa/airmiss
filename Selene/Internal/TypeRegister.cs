using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Selene.Core;
using Selene.Internal.Client;
using Selene.Internal.Middleware;
using Selene.Internal.Processor;
using Selene.Internal.Processor.Hub;
using Selene.Internal.Protocol;
using Selene.Messaging;

namespace Selene.Internal
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
                .AddSingleton<SeleneRunner>()
                .BuildServiceProvider(new ServiceProviderOptions
                {
                    ValidateOnBuild = true
                });
        }
    }
}
