using System;

namespace Airmiss.Processor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ProcessorHubAttribute : Attribute
    {
        public ProcessorHubAttribute() : this(string.Empty)
        {
        }

        public ProcessorHubAttribute(string routePrefix)
        {
            RoutePrefix = string.IsNullOrWhiteSpace(routePrefix)
                ? throw new ArgumentException($"Message processor hub {nameof(routePrefix)} cannot be empty")
                : new Route(routePrefix);
        }

        public Route RoutePrefix { get; set; }
    }
}