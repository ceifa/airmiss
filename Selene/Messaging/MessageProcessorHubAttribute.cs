using System;

namespace Selene.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MessageProcessorHubAttribute : Attribute
    {
        public MessageProcessorHubAttribute() : this(string.Empty)
        {
        }

        public MessageProcessorHubAttribute(string routePrefix)
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