using Airmiss.Processor;

namespace Airmiss.Messaging
{
    public class Message
    {
        public string? CorrelationId { get; set; }

        public Route Route { get; set; }

        public Verb Verb { get; set; }

        public object? Content { get; set; }
    }
}