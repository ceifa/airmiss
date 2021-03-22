using System;

namespace Airmiss.Internal.TypeActivator
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