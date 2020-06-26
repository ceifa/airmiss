using Selene.Core;
using System;

namespace Selene.Internal.Middleware
{
    internal interface IMiddlewareDescriptor
    {
        Type MiddlewareType { get; }

        bool ShouldRun(IProcessorDescriptor processorDescriptor);
    }
}
