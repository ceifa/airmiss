using System;
using System.Collections.Generic;
using System.Text;

namespace Selene.Messaging
{
    public class Message
    {
        public string Route { get; set; }

        public Verb Verb { get; set; }

        public object Content { get; set; }
    }
}
