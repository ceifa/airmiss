using System.Collections.Generic;

namespace Selene.Messaging
{
    public class ReceiverContext
    {
        internal ReceiverContext(MessageReceiver receiver)
        {
            MessageReceiver = receiver;
            Bag = new Dictionary<object, object>();
        }

        public MessageReceiver MessageReceiver { get; set; }

        public IDictionary<object, object> Bag { get; set; }
    }
}