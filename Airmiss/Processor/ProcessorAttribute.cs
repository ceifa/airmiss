using System;
using Airmiss.Messaging;

namespace Airmiss.Processor
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ProcessorAttribute : Attribute
    {
        public ProcessorAttribute(string route, Verb verb)
        {
            Route = string.IsNullOrWhiteSpace(route)
                ? throw new ArgumentException($"Message processor {nameof(route)} cannot be empty")
                : new Route(route);
            Verb = verb;
        }

        internal Route Route { get; set; }

        internal Verb Verb { get; set; }
    }
}