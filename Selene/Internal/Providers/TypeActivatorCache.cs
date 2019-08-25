using System;
using System.Collections.Concurrent;

namespace Selene.Internal.Providers
{
    internal class TypeActivatorCache : ITypeActivatorCache
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<Type, object> _typeActivatorCache =
            new ConcurrentDictionary<Type, object>();

        internal TypeActivatorCache(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetInstance(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var instance = _serviceProvider?.GetService(type);

            return instance ?? _typeActivatorCache.GetOrAdd(type, Activator.CreateInstance);
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