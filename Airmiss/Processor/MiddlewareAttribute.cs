using System;
using Airmiss.Core;

namespace Airmiss.Processor
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class MiddlewareAttribute : Attribute
    {
        public MiddlewareAttribute(Type middlewareType)
        {
            if (!middlewareType.IsAssignableFrom(typeof(IMiddleware)))
                throw new ArgumentException($"Middleware '{middlewareType.Name}' is not of type {nameof(IMiddleware)}");

            MiddlewareType = middlewareType;
        }

        public Type MiddlewareType { get; }
    }
}