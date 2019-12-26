using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Selene.Exceptions;
using Selene.Messaging;

namespace Selene.Internal.Processor
{
    internal class MessageProcessorProvider : IMessageProcessorProvider
    {
        private readonly IDictionary<Verb, ProcessorDescriptor[]> _messageProcessors;

        public MessageProcessorProvider(IEnumerable<ProcessorDescriptor> messageProcessors)
        {
            _messageProcessors = messageProcessors
                .GroupBy(d => d.Verb).ToDictionary(d => d.Key, d => d.ToArray());
        }

        public MessageProcessorContext GetProcessorContext(Verb verb, Route route)
        {
            route.EnsureIsValid();

            var match = _messageProcessors[verb]
                .SelectMany(m => m.Routes.Select(r => new { Route = r, Descriptor = m }))
                .FirstOrDefault(m => Route.Match(route, m.Route));

            if (match == default)
                throw new ProcessorNotFoundException($"No processor was found with route '{route}' and verb '{verb}'");

            return new MessageProcessorContext
            {
                Parameters = match.Route.GetVariableValues(route)
            };
        }
    }
}
