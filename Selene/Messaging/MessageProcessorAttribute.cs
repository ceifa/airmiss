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

        public string Path { get; set; }

        public Verb Verb { get; set; }
    }
}