using Selene.Internal;
using System;

namespace Selene.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MessageProcessorHubAttribute : Attribute
    {
        public MessageProcessorHubAttribute() : this(string.Empty)
        {
        }

        public MessageProcessorHubAttribute(string pathPrefix)
        {
            PathPrefix = pathPrefix;
        }

        public string PathPrefix { get; set; }

        internal Route AsRoute() => new Route(PathPrefix);
    }
}