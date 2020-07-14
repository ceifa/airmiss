namespace Selene.Messaging
{
    public class Message
    {
        public Route Route { get; set; }

        public Verb Verb { get; set; }

        public object? Content { get; set; }
    }
}