using System;
using Selene.Core;

namespace Selene.Configuration
{
    public class MiddlewareConfiguration
    {
        private readonly Action<Type> _addMiddleware;
        private readonly SeleneConfiguration _seleneConfiguration;

        internal MiddlewareConfiguration(
            SeleneConfiguration seleneConfiguration,
            Action<Type> addMiddleware)
        {
            _seleneConfiguration = seleneConfiguration ?? throw new ArgumentNullException(nameof(seleneConfiguration));
            _addMiddleware = addMiddleware ?? throw new ArgumentNullException(nameof(addMiddleware));
        }

        public SeleneConfiguration Add<TMiddleware>() where TMiddleware : IMiddleware
        {
            _addMiddleware(typeof(TMiddleware));
            return _seleneConfiguration;
        }
    }
}