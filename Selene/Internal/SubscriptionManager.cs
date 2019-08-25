using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Selene.Internal
{
    internal class SubscriptionManager : ISubscriptionManager
    {
        private readonly ConcurrentDictionary<string, ConcurrentBag<string>> _subscriptions;

        public SubscriptionManager()
        {
            _subscriptions = new ConcurrentDictionary<string, ConcurrentBag<string>>();
        }

        public void Subscribe(string key, string subscriberConnectionId)
        {
            _subscriptions.AddOrUpdate(key, k => new ConcurrentBag<string>
            {
                subscriberConnectionId
            }, (_, bag) =>
            {
                bag.Add(subscriberConnectionId);
                return bag;
            });
        }

        public void Unsubscribe(string key, string subscriberConnectionId)
        {
            _subscriptions.AddOrUpdate(key, k => new ConcurrentBag<string>(),
                (_, bag) => new ConcurrentBag<string>(bag.Where(c => c != subscriberConnectionId)));
        }

        public string[] GetSubscribers(string key)
        {
            return _subscriptions.TryGetValue(key, out var subscribers) && subscribers != null
                ? subscribers.ToArray() : Array.Empty<string>();
        }
    }
}
