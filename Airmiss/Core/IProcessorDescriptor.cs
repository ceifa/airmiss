using System;
using System.Reflection;
using Airmiss.Processor;

namespace Airmiss.Core
{
    public interface IProcessorDescriptor
    {
        Type HubType { get; }

        Route[] Routes { get; }

        Verb Verb { get; }

        MethodInfo ProcessorMethod { get; }
    }
}