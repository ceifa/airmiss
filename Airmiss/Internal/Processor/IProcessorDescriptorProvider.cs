using System.Collections.Generic;
using Airmiss.Core;
using Airmiss.Processor;

namespace Airmiss.Internal.Processor
{
    internal interface IProcessorDescriptorProvider
    {
        IProcessorDescriptor GetProcessorContext(Verb verb, Route route, out IDictionary<string, string> pathVariables);
    }
}