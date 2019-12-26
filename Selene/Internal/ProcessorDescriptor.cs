using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Selene.Messaging;

namespace Selene.Internal
{
    internal class ProcessorDescriptor
    {
        internal ProcessorDescriptor(Type hubType, IEnumerable<Route> routes, Verb verb, MethodInfo processorMethod)
        {
            HubType = hubType;
            Routes = routes?.ToArray();
            Verb = verb;
            ProcessorMethod = processorMethod;
        }

        public Type HubType { get; }

        public Route[] Routes { get; }

        public Verb Verb { get; }

        public MethodInfo ProcessorMethod { get; }
    }
}