using System;

namespace Selene.Messaging
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MessageProcessorAttribute : Attribute
    {
        public MessageProcessorAttribute(string route, Verb verb)
        {
            Route = !string.IsNullOrWhiteSpace(route)
                ? route
                : throw new ArgumentException($"Message processor {nameof(route)} cannot be empty");
            Verb = verb;
        }

        internal string Route { get; set; }

        internal Verb Verb { get; set; }

        internal Route GetRoute()
        {
            return new Route(Route);
        }
    }
}