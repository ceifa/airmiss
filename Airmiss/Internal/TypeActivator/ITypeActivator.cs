using System;

namespace Airmiss.Internal.TypeActivator
{
    internal interface ITypeActivator
    {
        object? GetInstance(Type type);

        void Release(object instance);
    }
}