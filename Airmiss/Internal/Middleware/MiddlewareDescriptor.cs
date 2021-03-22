using System;
using Airmiss.Core;

namespace Airmiss.Internal.Middleware
{
    internal class MiddlewareDescriptor : IMiddlewareDescriptor
    {
        private readonly Predicate<IProcessorDescriptor>? _shouldRun;

        public MiddlewareDescriptor(Type middlewareType, Predicate<IProcessorDescriptor>? shouldRun)
        {
            _shouldRun = shouldRun;
            MiddlewareType = middlewareType;
        }

        public MiddlewareDescriptor(Type middlewareType) : this(middlewareType, null)
        {
        }

        public Type MiddlewareType { get; }

        public bool ShouldRun(IProcessorDescriptor processorDescriptor)
        {
            return _shouldRun == null || _shouldRun(processorDescriptor);
        }
    }
}