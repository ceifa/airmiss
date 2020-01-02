using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Selene.Configuration;
using Selene.Internal;
using Selene.Internal.Processor.Hub;
using Selene.Messaging;

namespace Selene
{
    public class SeleneConfiguration
    {
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
            _serviceCollection.TryAddSingleton<ITypeActivator>(new ServiceBasedTypeActivator(serviceProvider));
            return this;
        }

        public SeleneRunner GetRunner()
        {
            if (_serviceCollection.Any(s => s.ServiceType.Equals(typeof(IMessageProtocol))))
                throw new InvalidOperationException($"At least one {nameof(Protocol)} should be defined");

            var provider = TypeRegister.RegisterTypesAndGetProvider(_serviceCollection);
            return provider.GetRequiredService<SeleneRunner>();
        }

        private void Add<T>(T instance) where T : class => _serviceCollection.TryAddSingleton(instance);
    }
}