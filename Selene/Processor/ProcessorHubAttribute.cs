using System;
using Selene.Messaging;

namespace Selene.Processor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ProcessorHubAttribute : Attribute
    {
        public ProcessorHubAttribute() : this(string.Empty)
        {
        }

        public ProcessorHubAttribute(string routePrefix)
        {
            RoutePrefix = routePrefix;
        }

        public string RoutePrefix { get; set; }

        internal Route GetRoute()
        {
            return new Route(RoutePrefix);
        }
    }
}