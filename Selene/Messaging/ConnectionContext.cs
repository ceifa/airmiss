using System;
using System.Collections.Generic;
using System.Text;

namespace Selene.Messaging
{
    public class ConnectionContext
    {
        private string ConnectionId { get; set; }

        public IDictionary<object, object> Items { get; set; }
    }
}
