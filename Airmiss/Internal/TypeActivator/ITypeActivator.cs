using System;

namespace Airmiss.Internal
{
    internal interface ITypeActivator
    {
        object? GetInstance(Type type);

        void Release(object instance);
    }
}