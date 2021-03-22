using System;
using System.Collections.Concurrent;

namespace Airmiss.Internal.TypeActivator
{
    internal class CachedTypeActivator : ITypeActivator
    {
        private readonly Func<Type, object?> _createInstance = Activator.CreateInstance;
        private readonly ConcurrentDictionary<Type, object?> _typeCache = new();

        public object? GetInstance(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsAssignableFrom(typeof(IDisposable))
                ? _createInstance(type)
                : _typeCache.GetOrAdd(type, _createInstance);
        }

        public void Release(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (instance is IDisposable disposable)
                disposable.Dispose();
        }
    }
}