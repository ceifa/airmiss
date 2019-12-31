using System;
using Selene.Core;

namespace Selene.Configuration
{
    public class MiddlewareConfiguration
    {
        private readonly Action<IMiddleware> _addMiddleware;
        private readonly SeleneConfiguration _seleneConfiguration;

        internal MiddlewareConfiguration(
            SeleneConfiguration seleneConfiguration,
            Action<IMiddleware> addMiddleware)
        {
            _seleneConfiguration = seleneConfiguration ?? throw new ArgumentNullException(nameof(seleneConfiguration));
            _addMiddleware = addMiddleware ?? throw new ArgumentNullException(nameof(addMiddleware));
        }

        public SeleneConfiguration Add<TMiddleware>(TMiddleware middleware) where TMiddleware : IMiddleware
        {
            if (middleware == null)
                throw new ArgumentNullException(nameof(middleware));

            _addMiddleware(middleware);
            return _seleneConfiguration;
        }
    }
}