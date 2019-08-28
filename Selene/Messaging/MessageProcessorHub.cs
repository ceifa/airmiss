using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Selene.Internal;

namespace Selene.Messaging
{
    public class MessageProcessorHub
    {
        private static readonly AsyncLocal<ConnectionContext> CurrentConnectionContext =
            new AsyncLocal<ConnectionContext>();

        internal ConnectionContext Context
        {
            get => CurrentConnectionContext?.Value;
            set
            {
                if (value != null) CurrentConnectionContext.Value = value;
            }
        }

        internal ISubscriptionManager SubscriptionManager { private get; set; }

        internal IMessageProtocol[] MessageProtocols { private get; set; }

        protected void Subscribe(string key)
        {
            SubscriptionManager.Subscribe(key, Context.ConnectionId);
        }

        protected void Unsubscribe(string key)
        {
            SubscriptionManager.Unsubscribe(key, Context.ConnectionId);
        }

        protected Task SendToSubscribersAsync<T>(string key, T content, CancellationToken cancellationToken)
        {
            var subscribers = SubscriptionManager.GetSubscribers(key);
            return Task.WhenAll(subscribers.SelectMany(s =>
                MessageProtocols.Select(m => m.SendAsync(s, content, cancellationToken))));
        }
    }
}