using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Selene.Internal
{
    internal interface ISubscriptionManager
    {
        void Subscribe(string key, string subscriberConnectionId);

        void Unsubscribe(string key, string subscriberConnectionId);

        string[] GetSubscribers(string key);
    }
}
