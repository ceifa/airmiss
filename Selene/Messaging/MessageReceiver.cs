namespace Selene.Messaging
{
    public class MessageReceiver
    {
        public MessageReceiver(string identity)
        {
            Identity = identity;
        }

        public string Identity { get; }
    }
}
