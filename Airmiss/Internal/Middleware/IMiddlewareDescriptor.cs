using System;
using Airmiss.Core;

namespace Airmiss.Internal.Middleware
{
    internal interface IMiddlewareDescriptor
    {
        Type MiddlewareType { get; }

        bool ShouldRun(IProcessorDescriptor processorDescriptor);
    }
}