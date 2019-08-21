using Selene.Internal;
using Selene.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Selene
{
    internal class MessageProcessorDescriptor
    {
        internal MessageProcessorDescriptor(Type hubType, IEnumerable<Route> routes, Verb verb,
            MethodInfo messageProcessor)
        {
            HubType = hubType ?? throw new ArgumentNullException(nameof(hubType));
            Routes = routes?.ToArray() ?? throw new ArgumentNullException(nameof(routes));
            Verb = verb;
            MessageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
        }

        public Type HubType { get; }

        public Route[] Routes { get; }

        public Verb Verb { get; }

        public MethodInfo MessageProcessor { get; }
    }
}