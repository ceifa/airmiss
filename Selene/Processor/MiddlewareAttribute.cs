using System;
using Selene.Core;

namespace Selene.Processor
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
