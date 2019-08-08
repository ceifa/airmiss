using System;

namespace Selene.Messaging
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MessageProcessorAttribute : Attribute
    {
        public MessageProcessorAttribute(string path, Verb verb)
        {
            Path = !string.IsNullOrWhiteSpace(path)
                ? path
                : throw new ArgumentException("Message processor path cannot be empty");
            Verb = verb;
        }

        internal string Path { get; set; }

        internal Verb Verb { get; set; }
    }
}