using System.Collections.Generic;

namespace Selene.Internal
{
    internal class MessageProcessorContext
    {
        internal MessageProcessorContext(MessageProcessorDescriptor messageProcessorDescriptor,
            Dictionary<string, string> variables)
        {
            MessageProcessorDescriptor = messageProcessorDescriptor;
            Variables = variables;
        }

        internal MessageProcessorDescriptor MessageProcessorDescriptor { get; set; }

        internal Dictionary<string, string> Variables { get; set; }
    }
}