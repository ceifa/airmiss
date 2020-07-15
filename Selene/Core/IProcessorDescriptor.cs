using System;
using System.Reflection;
using Selene.Processor;

namespace Selene.Core
{
    public interface IProcessorDescriptor
    {
        Type HubType { get; }

        Route[] Routes { get; }

        Verb Verb { get; }

        MethodInfo ProcessorMethod { get; }
    }
}
