using System;

namespace Selene.Providers
{
    internal interface ITypeActivatorCache
    {
        object GetInstance(Type type);

        void Release(object instance);
    }
}