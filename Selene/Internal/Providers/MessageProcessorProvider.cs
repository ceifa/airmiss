using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Selene.Exceptions;
using Selene.Messaging;

namespace Selene.Internal.Providers
{
    internal class MessageProcessorProvider : IMessageProcessorProvider
    {
        private readonly IDictionary<Verb, MessageProcessorDescriptor[]> _messageProcessorDescriptors;

        internal MessageProcessorProvider(IEnumerable<MessageProcessorDescriptor> messageProcessorDescriptors)
        {
            if (messageProcessorDescriptors == null)
                throw new ArgumentNullException(nameof(messageProcessorDescriptors));

            _messageProcessorDescriptors = messageProcessorDescriptors
                .GroupBy(d => d.Verb).ToDictionary(d => d.Key, d => d.ToArray());
        }

        public MessageProcessorContext GetProcessor(Route route, Verb verb)
        {
            var match = _messageProcessorDescriptors[verb]
                .SelectMany(m => m.Routes.Select(r => new { Route = r, Descriptor = m }))
                .FirstOrDefault(m => Route.Match(route, m.Route));

            if (match == default)
                throw new ProcessorNotFoundException($"No processor was found with route '{route}' and verb '{verb}'");

            return new MessageProcessorContext(match.Descriptor, match.Route.GetVariables(route));
        }
    }
}