using System;

namespace Selene.Internal.Providers
{
    internal interface ITypeActivatorCache
    {
        object GetInstance(Type type);

        void Release(object instance);
    }
}