using System.Collections.Generic;
using System.Reflection;

namespace Selene.Internal.Processor
{
    internal class MessageProcessorContext
    {
        public object HubInstance { get; set; }

        public MethodInfo ProcessorMethod { get; set; }

        public Dictionary<string, string> ParametersArguments { get; set; }
    }
}
