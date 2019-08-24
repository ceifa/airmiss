﻿using Selene.Internal;
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
                        var hubMethodRoute = hubMethodAttribute.AsRoute();

                        var routes = hubTypeAttributes
                            .Select(attribute => attribute.AsRoute() + hubMethodRoute);

                        Add(hubType, routes, hubMethodAttribute.Verb, hubMethod);
                    }
                }
            }

            return _seleneConfiguration;
        }

        public SeleneConfiguration Add<THub>(IEnumerable<Route> routes, Verb verb, MethodInfo messageProcessor)
        {
            return Add(typeof(THub), routes, verb, messageProcessor);
        }

        public SeleneConfiguration Add(Type hubType, IEnumerable<Route> routes, Verb verb, MethodInfo messageProcessor)
        {
            if (hubType == null)
                throw new ArgumentNullException(nameof(hubType));

            if (routes == null)
                throw new ArgumentNullException(nameof(routes));

            if (messageProcessor == null)
                throw new ArgumentNullException(nameof(messageProcessor));

            var descriptor = new MessageProcessorDescriptor(hubType, routes.Select(r => r.EnsureIsValid()), verb, messageProcessor);

            _addMessageProcessor(descriptor);

            return _seleneConfiguration;
        }
    }
}