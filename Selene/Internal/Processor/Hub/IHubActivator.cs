using System;

namespace Selene.Internal
{
    internal interface IHubActivator
    {
        object GetInstance(Type type);

        void Release(object instance);
    }
}