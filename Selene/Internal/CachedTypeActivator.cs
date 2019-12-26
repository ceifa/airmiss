using System;
using System.Collections.Concurrent;

namespace Selene.Internal
{
    internal class CachedTypeActivator : ITypeActivator
    {
        private readonly ConcurrentDictionary<Type, object> _typeCache = new ConcurrentDictionary<Type, object>();
        private readonly IServiceProvider _serviceProvider;

        internal CachedTypeActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetInstance(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var instance = _serviceProvider?.GetService(type);

            return instance ?? _typeCache.GetOrAdd(type, Activator.CreateInstance);
        }

        public void Release(object instance)
        {
            switch (instance)
            {
                case null:
                    throw new ArgumentNullException(nameof(instance));
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }
    }
}
