using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Selene.Internal;
using Selene.Messaging;
using Selene.Processor;

namespace Selene.Configuration
{
    public class ProcessorConfiguration
    {
        private readonly Action<ProcessorDescriptor> _addProcessor;
        private readonly SeleneConfiguration _seleneConfiguration;

        internal ProcessorConfiguration(
            SeleneConfiguration seleneConfiguration,
            Action<ProcessorDescriptor> addProcessor)
        {
            _seleneConfiguration = seleneConfiguration ?? throw new ArgumentNullException(nameof(seleneConfiguration));
            _addProcessor = addProcessor ?? throw new ArgumentNullException(nameof(addProcessor));
        }

        public SeleneConfiguration AddHub<THub>()
        {
            return AddHub(typeof(THub));
        }

        public SeleneConfiguration AddHub(params Type[] hubTypes)
        {
            if (hubTypes == null)
                throw new ArgumentNullException(nameof(hubTypes));

            foreach (var hubType in hubTypes)
            {
                AddHub(hubType);
            }

            return _seleneConfiguration;
        }

        public SeleneConfiguration AddHub(Type hubType)
        {
            if (hubType == null)
                throw new ArgumentNullException(nameof(hubType));

            var hubTypeAttributes = hubType.GetCustomAttributes<ProcessorHubAttribute>().ToArray();

            foreach (var hubMethod in hubType.GetMethods())
            {
                var hubMethodAttributes = hubMethod.GetCustomAttributes<ProcessorAttribute>();

                foreach (var hubMethodAttribute in hubMethodAttributes)
                {
                    var hubMethodRoute = hubMethodAttribute.GetRoute();

                    var routes = hubTypeAttributes
                        .Select(attribute => attribute.GetRoute() + hubMethodRoute);

                    Add(hubType, routes, hubMethodAttribute.Verb, hubMethod);
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

            var descriptor = new ProcessorDescriptor(
                hubType, routes.Select(r => r.EnsureIsValid()), verb, messageProcessor);

            _addProcessor(descriptor);

            return _seleneConfiguration;
        }
    }
}