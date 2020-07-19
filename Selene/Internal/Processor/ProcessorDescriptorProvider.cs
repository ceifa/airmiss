using System.Collections.Generic;
using System.Linq;
using Selene.Core;
using Selene.Exceptions;
using Selene.Processor;

namespace Selene.Internal.Processor
{
    internal class ProcessorDescriptorProvider : IProcessorDescriptorProvider
    {
        private readonly IDictionary<Verb, IProcessorDescriptor[]> _messageProcessors;

        public ProcessorDescriptorProvider(IEnumerable<IProcessorDescriptor> messageProcessors)
        {
            _messageProcessors = messageProcessors
                .GroupBy(d => d.Verb).ToDictionary(d => d.Key, d => d.ToArray());
        }

        public IProcessorDescriptor GetProcessorContext(Verb verb, Route route,
            out IDictionary<string, string> pathVariables)
        {
            route.EnsureIsValid();

            var match = _messageProcessors[verb]
                .SelectMany(m => m.Routes.Select(r => new {Route = r, Descriptor = m}))
                .FirstOrDefault(m => Route.Match(m.Route, route));

            if (match == default)
                throw new ProcessorNotFoundException($"No processor was found with route '{route}' and verb '{verb}'");

            pathVariables = match.Route.GetVariableValues(route);
            return match.Descriptor;
        }
    }
}