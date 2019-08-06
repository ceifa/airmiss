using System.Collections.Generic;

namespace Selene.Messaging
{
    public class ConnectionContext
    {
        internal ConnectionContext(string connectionId)
        {
            ConnectionId = connectionId;
        }

        public string ConnectionId { get; set; }

        public IDictionary<object, object> Bag { get; set; }
    }
}
