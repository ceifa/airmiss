using Selene.Exceptions;
using Selene.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Selene.Providers
{
    internal class MessageProcessorDescriptorProvider : IMessageProcessorDescriptorProvider
    {
        private static readonly Regex RouteVariableRegex = new Regex(@"^\{(.*?)\}$", RegexOptions.Compiled);

        private readonly MessageProcessorDescriptor[] _messageProcessorDescriptors;

        internal MessageProcessorDescriptorProvider(IEnumerable<MessageProcessorDescriptor> messageProcessorDescriptors)
        {
            _messageProcessorDescriptors = messageProcessorDescriptors?.ToArray() ??
                                           throw new ArgumentNullException(nameof(messageProcessorDescriptors));
        }

        public MessageProcessorContext GetDescriptor(string route, Verb verb)
        {
            var paths = route.Split('/');
            var matches = (from messageProcessorDescriptor in _messageProcessorDescriptors
                           where messageProcessorDescriptor.Verb.Equals(verb)
                           let variablePaths = new Dictionary<string, string>()
                           from messageProcessorPath in messageProcessorDescriptor.Paths
                           let match = paths.All(path =>
                           {
                               var variableName = GetRouteVariableName(path);

                               if (variableName == null)
                                   return path == messageProcessorPath;

                               variablePaths.Add(variableName.ToLowerInvariant(), path);
                               return true;
                           })
                           where match
                           select new MessageProcessorContext(messageProcessorDescriptor, variablePaths)).ToArray();

            switch (matches.Length)
            {
                case 0:
                    throw new ProcessorNotFoundException($"Route '{route}' not found");
                case 1:
                    return matches[0];
                default:
                    throw new InvalidOperationException($"Route '{route}' matched more than one message processor");
            }
        }

        private static string GetRouteVariableName(string path)
        {
            var routeVariableMatch = RouteVariableRegex.Match(path);

            if (!routeVariableMatch.Success || routeVariableMatch.Groups.Count != 1) return null;

            return routeVariableMatch.Groups[1].Value;
        }
    }
}