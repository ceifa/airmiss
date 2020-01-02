using System.Collections.Generic;
using Selene.Core;
using Selene.Messaging;

namespace Selene.Internal.Processor
{
    internal interface IProcessorDescriptorProvider
    {
        IProcessorDescriptor GetProcessorContext(Verb verb, Route route, out IDictionary<string, string> pathVariables);
    }
}