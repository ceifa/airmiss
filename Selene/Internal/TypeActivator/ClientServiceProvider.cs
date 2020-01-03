using System;

namespace Selene.Internal.TypeActivator
{
    internal class ClientServiceProvider : IClientServiceProvider
    {
        public ClientServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
