using Airmiss.Core;
using System;

namespace Airmiss.Internal.Middleware
{
    internal interface IMiddlewareDescriptor
    {
        Type MiddlewareType { get; }

        bool ShouldRun(IProcessorDescriptor processorDescriptor);
    }
}
