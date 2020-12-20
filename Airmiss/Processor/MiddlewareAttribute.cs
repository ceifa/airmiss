using System;
using Airmiss.Core;

namespace Airmiss.Processor
{
    public class MiddlewareAttribute : Attribute
    {
        public Type MiddlewareType { get; }

        public MiddlewareAttribute(Type middlewareType)
        {
            if (!middlewareType.IsAssignableFrom(typeof(IMiddleware)))
                throw new ArgumentException($"Middleware '{middlewareType.Name}' is not of type {nameof(IMiddleware)}");

            MiddlewareType = middlewareType;
        }
    }
}
