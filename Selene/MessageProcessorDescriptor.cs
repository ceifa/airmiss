using Selene.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Selene
{
    internal class MessageProcessorDescriptor
    {
        internal MessageProcessorDescriptor(Type hubType, IEnumerable<string> paths, Verb verb,
            MethodInfo messageProcessor)
        {
            HubType = hubType ?? throw new ArgumentNullException(nameof(hubType));
            Paths = paths?.ToArray() ?? throw new ArgumentNullException(nameof(paths));
            Verb = verb;
            MessageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
        }

        public Type HubType { get; }

        public string[] Paths { get; }

        public Verb Verb { get; }

        public MethodInfo MessageProcessor { get; }
    }
}