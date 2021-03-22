using System;

namespace Airmiss.Internal.TypeActivator
{
    internal interface IClientServiceProvider
    {
        IServiceProvider ServiceProvider { get; }
    }
}