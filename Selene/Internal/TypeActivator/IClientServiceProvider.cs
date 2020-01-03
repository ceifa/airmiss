using System;

namespace Selene.Internal.TypeActivator
{
    internal interface IClientServiceProvider
    {
        IServiceProvider ServiceProvider { get; }
    }
}
