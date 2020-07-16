using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Selene.Core;
using Selene.Internal.Processor;
using Selene.Processor;

namespace Selene.Configuration
{
    public class ProcessorConfiguration
    {
        private readonly Action<IProcessorDescriptor> _addProcessor;
        private readonly SeleneConfiguration _seleneConfiguration;

        internal ProcessorConfiguration(
            SeleneConfiguration seleneConfiguration,
            Action<IProcessorDescriptor> addProcessor)
        {
            _seleneConfiguration = seleneConfiguration ?? throw new ArgumentNullException(nameof(seleneConfiguration));
            _addProcessor = addProcessor ?? throw new ArgumentNullException(nameof(addProcessor));
        }

        public SeleneConfiguration AddCurrentAssembly()
        {
            return AddAssembly(Assembly.GetCallingAssembly());
        }

        public SeleneConfiguration AddAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            return AddHub(assembly.GetTypes().Where(type => type.IsClass).ToArray());
        }

        public SeleneConfiguration AddHub<THub>()
        {
            return AddHub(typeof(THub));
        }

        public SeleneConfiguration AddHub(params Type[] hubTypes)
        {
            if (hubTypes == null)
                throw new ArgumentNullException(nameof(hubTypes));

            foreach (var hubType in hubTypes) AddHub(hubType);

            return _seleneConfiguration;
        }

        public SeleneConfiguration AddHub(Type hubType)
        {
            if (hubType == null)
                throw new ArgumentNullException(nameof(hubType));

            foreach (var hubMethod in hubType.GetMethods())
            {
                Add(hubType, hubMethod);
            }

            return _seleneConfiguration;
        }

        public SeleneConfiguration Add(Type hubType, MethodInfo processor)
        {
            if (hubType == null)
                throw new ArgumentNullException(nameof(hubType));

            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            var hubTypeAttributes = hubType.GetCustomAttributes<ProcessorHubAttribute>().ToArray();
            var hubMethodAttributes = processor.GetCustomAttributes<ProcessorAttribute>();

            foreach (var hubMethodAttribute in hubMethodAttributes)
            {
                var hubMethodRoute = hubMethodAttribute.Route;

                var routes = hubTypeAttributes.Length == 0 ? new[] { hubMethodRoute } :
                    hubTypeAttributes.Select(attribute => attribute.RoutePrefix + hubMethodRoute);

                Add(hubType, routes, hubMethodAttribute.Verb, processor, GetLocalMiddlewares(hubType, processor));
            }

            return _seleneConfiguration;
        }


        private void Add(Type hubType, IEnumerable<Route> routes, Verb verb, MethodInfo processor, IEnumerable<Type> middlewares)
        {
            if (hubType == null)
                throw new ArgumentNullException(nameof(hubType));

            if (routes == null)
                throw new ArgumentNullException(nameof(routes));

            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            if (middlewares == null)
                throw new ArgumentNullException(nameof(middlewares));

            var descriptor = new ProcessorDescriptor(hubType, routes.Select(r => r.EnsureIsValid()), verb, processor);

            bool LocalMiddlewareShouldRun(IProcessorDescriptor processorDescriptor) => processorDescriptor.Equals(descriptor);
            foreach (var middleware in middlewares)
            {
                _seleneConfiguration.Middleware.Add(middleware, LocalMiddlewareShouldRun);
            }

            _addProcessor(descriptor);
        }

        private static IEnumerable<Type> GetLocalMiddlewares(MemberInfo hubType, MemberInfo processor)
        {
            return hubType.GetCustomAttributes<MiddlewareAttribute>()
                .Concat(processor.GetCustomAttributes<MiddlewareAttribute>())
                .Select(m => m.MiddlewareType);
        }
    }
}