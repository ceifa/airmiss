using System;
using Airmiss.Core;
using Airmiss.Internal.Middleware;

namespace Airmiss.Configuration
{
    public class MiddlewareConfiguration
    {
        private readonly Action<IMiddlewareDescriptor> _addMiddleware;
        private readonly AirmissConfiguration _AirmissConfiguration;

        internal MiddlewareConfiguration(
            AirmissConfiguration AirmissConfiguration,
            Action<IMiddlewareDescriptor> addMiddleware)
        {
            _AirmissConfiguration = AirmissConfiguration ?? throw new ArgumentNullException(nameof(AirmissConfiguration));
            _addMiddleware = addMiddleware ?? throw new ArgumentNullException(nameof(addMiddleware));
        }

        public AirmissConfiguration Add<TMiddleware>(Predicate<IProcessorDescriptor>? shouldRun = null) where TMiddleware : IMiddleware
        {
            return Add(typeof(TMiddleware), shouldRun);
        }

        public AirmissConfiguration Add(Type middlewareType, Predicate<IProcessorDescriptor>? shouldRun = null)
        {
            if (!middlewareType.IsAssignableFrom(typeof(IMiddleware)))
            {
                throw new ArgumentException($"Middleware '{middlewareType.Name}' is not of type {nameof(IMiddleware)}");
            }

            _addMiddleware(new MiddlewareDescriptor(middlewareType, shouldRun));
            return _AirmissConfiguration;
        }
    }
}