using System.Collections.Generic;
using Selene.Messaging;

namespace Selene.Internal.Processor
{
    internal interface IProcessorDescriptorProvider
    {
        ProcessorDescriptor GetProcessorContext(Verb verb, Route route, out IDictionary<string, string> pathVariables);
    }
}