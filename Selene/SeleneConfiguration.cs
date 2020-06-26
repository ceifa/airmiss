using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Selene.Configuration;
using Selene.Internal;
using Selene.Internal.Processor.Hub;
using Selene.Internal.TypeActivator;

namespace Selene
{
    public class SeleneConfiguration
    {
        private bool _runnerWasCreated;

        private readonly IServiceCollection _serviceCollection;

        public SeleneConfiguration()
        {
            _serviceCollection = new ServiceCollection();

            Processor = new ProcessorConfiguration(this, Add);
            Protocol = new ProtocolConfiguration(this, Add);
            Middleware = new MiddlewareConfiguration(this, Add);
        }

        public ProcessorConfiguration Processor { get; internal set; }

        public ProtocolConfiguration Protocol { get; internal set; }

        public MiddlewareConfiguration Middleware { get; internal set; }

        public SeleneConfiguration UsingServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceCollection.TryAddScoped<IClientServiceProvider>(_ => new ClientServiceProvider(serviceProvider));
            _serviceCollection.TryAddScoped<ITypeActivator, ServiceBasedTypeActivator>();
            return this;
        }

        public SeleneRunner GetRunner()
        {
            // TODO: Possibility to build runners without referencing this configuration
            if (_runnerWasCreated)
            {
                throw new InvalidOperationException("Cannot create more than one runner for this configuration");
            }

            try
            {
                var provider = TypeRegister.RegisterTypesAndGetProvider(_serviceCollection);
                return provider.GetRequiredService<SeleneRunner>();
            }
            finally
            {
                _runnerWasCreated = true;
            }
        }

        private void Add<T>(T instance) where T : class => _serviceCollection.TryAddSingleton(instance);
    }
}