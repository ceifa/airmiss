using Selene.Internal;
using Selene.Internal.Extensions;
using Selene.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Selene.Configuration
{
    public class MessageProcessorConfiguration
    {
        private readonly Action<MessageProcessorDescriptor> _addMessageProcessor;
        private readonly SeleneConfiguration _seleneConfiguration;

        internal MessageProcessorConfiguration(SeleneConfiguration seleneConfiguration,
            Action<MessageProcessorDescriptor> addMessageProcessor)
        {
            _seleneConfiguration = seleneConfiguration ?? throw new ArgumentNullException(nameof(seleneConfiguration));
            _addMessageProcessor = addMessageProcessor ?? throw new ArgumentNullException(nameof(addMessageProcessor));
        }

        public SeleneConfiguration AddHub<THub>()
        {
            return AddHub(typeof(THub));
        }

        public SeleneConfiguration AddHub(Type hubType)
        {
            if (hubType == null)
                throw new ArgumentNullException(nameof(hubType));

            return AddHub(new[] { hubType });
        }

        public SeleneConfiguration AddHub(params Type[] hubTypes)
        {
            if (hubTypes == null)
                throw new ArgumentNullException(nameof(hubTypes));

            foreach (var hubType in hubTypes)
            {
                if (hubType == null)
                    throw new ArgumentException($"A {nameof(hubType)} cannot be null");

                var hubTypeAttributes = hubType.GetCustomAttributes<MessageProcessorHubAttribute>().ToArray();

                foreach (var hubMethod in hubType.GetMethods())
                {
                    var hubMethodAttributes = hubMethod.GetCustomAttributes<MessageProcessorAttribute>();

                    foreach (var hubMethodAttribute in hubMethodAttributes)
                    {
                        var paths = hubTypeAttributes
                            .Select(attribute => Path.Combine(attribute.PathPrefix, hubMethodAttribute.Path)).ToArray();

                        if (paths.Any(string.IsNullOrWhiteSpace))
                            throw new ArgumentException($"Hub ${hubType.FullName} has empty path methods");

                        Add(hubType, paths, hubMethodAttribute.Verb, hubMethod);
                    }
                }
            }

            return _seleneConfiguration;
        }

        public SeleneConfiguration Add<THub>(IEnumerable<string> paths, Verb verb, MethodInfo messageProcessor)
        {
            return Add(typeof(THub), paths, verb, messageProcessor);
        }

        public SeleneConfiguration Add(Type hubType, IEnumerable<string> paths, Verb verb, MethodInfo messageProcessor)
        {
            if (hubType == null)
                throw new ArgumentNullException(nameof(hubType));

            if (paths == null)
                throw new ArgumentNullException(nameof(paths));

            if (messageProcessor == null)
                throw new ArgumentNullException(nameof(messageProcessor));

            var routes = paths.Select(p => new Route(p).EnsureIsValid());
            var descriptor = new MessageProcessorDescriptor(hubType, routes, verb, messageProcessor);

            _addMessageProcessor(descriptor);

            return _seleneConfiguration;
        }
    }
}