using System;
using Airmiss.Configuration;
using Airmiss.Internal;
using Airmiss.Internal.Processor.Hub;
using Airmiss.Internal.TypeActivator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Airmiss
{
    public sealed class AirmissConfiguration
    {
        private readonly IServiceCollection _serviceCollection;

        public AirmissConfiguration()
        {
            _serviceCollection = new ServiceCollection();

            Processor = new ProcessorConfiguration(this, Add);
            Protocol = new ProtocolConfiguration(this, Add);
            Middleware = new MiddlewareConfiguration(this, Add);
        }

        public ProcessorConfiguration Processor { get; internal set; }

        public ProtocolConfiguration Protocol { get; internal set; }

        public MiddlewareConfiguration Middleware { get; internal set; }

        public AirmissConfiguration UsingServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceCollection.TryAddScoped<IClientServiceProvider>(_ => new ClientServiceProvider(serviceProvider));
            _serviceCollection.TryAddScoped<ITypeActivator, ServiceBasedTypeActivator>();
            return this;
        }

        public AirmissRunner GetRunner()
        {
            var provider = TypeRegister.RegisterTypesAndGetProvider(_serviceCollection);
            return provider.GetRequiredService<AirmissRunner>();
        }

        private void Add<T>(T instance) where T : class
        {
            _serviceCollection.AddSingleton(instance);
        }
    }
}