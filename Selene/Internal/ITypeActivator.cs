using System;

namespace Selene.Internal
{
    internal interface ITypeActivator
    {
        object GetInstance(Type type);

        void Release(object instance);
    }
}
