using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Selene.Exceptions;
using Selene.Messaging;

namespace Selene.Internal.Providers
{
    internal class MessageProcessorDescriptorProvider : IMessageProcessorDescriptorProvider
    {
        private readonly MessageProcessorDescriptor[] _messageProcessorDescriptors;

        internal MessageProcessorDescriptorProvider(IEnumerable<MessageProcessorDescriptor> messageProcessorDescriptors)
        {
            _messageProcessorDescriptors = messageProcessorDescriptors?.ToArray() ??
                                           throw new ArgumentNullException(nameof(messageProcessorDescriptors));
        }

        public MessageProcessorContext GetDescriptor(Route route, Verb verb)
        {
            var match = _messageProcessorDescriptors.Where(m => m.Verb == verb)
                .SelectMany(m => m.Routes.Select(r => new {Route = r, Descriptor = m}))
                .FirstOrDefault(m => Route.Match(route, m.Route));

            if (match == default)
                throw new ProcessorNotFoundException($"No processor was found with route '{route}' and verb '{verb}'");

            return new MessageProcessorContext(match.Descriptor, match.Route.GetVariables(route));
        }
    }
}