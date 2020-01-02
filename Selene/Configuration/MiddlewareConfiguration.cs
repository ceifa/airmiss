using System;
using Selene.Core;
using Selene.Internal.Middleware;

namespace Selene.Configuration
{
    public class MiddlewareConfiguration
    {
        private readonly Action<MiddlewareDescriptor> _addMiddleware;
        private readonly SeleneConfiguration _seleneConfiguration;

        internal MiddlewareConfiguration(
            SeleneConfiguration seleneConfiguration,
            Action<MiddlewareDescriptor> addMiddleware)
        {
            _seleneConfiguration = seleneConfiguration ?? throw new ArgumentNullException(nameof(seleneConfiguration));
            _addMiddleware = addMiddleware ?? throw new ArgumentNullException(nameof(addMiddleware));
        }

        public SeleneConfiguration Add<TMiddleware>() where TMiddleware : IMiddleware
        {
            return Add(typeof(TMiddleware));
        }

        public SeleneConfiguration Add(Type middlewareType, Predicate<IProcessorDescriptor>? shouldRun = null)
        {
            if (!middlewareType.IsAssignableFrom(typeof(IMiddleware)))
            {
                throw new ArgumentException($"Middleware '{middlewareType.Name}' is not of type {nameof(IMiddleware)}");
            }

            _addMiddleware(new MiddlewareDescriptor(middlewareType, shouldRun));
            return _seleneConfiguration;
        }
    }
}