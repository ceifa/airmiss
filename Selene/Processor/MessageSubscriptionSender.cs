using Selene.Internal;
using Selene.Messaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Processor
{
    internal class MessageSubscriptionSender : IMessageSubscriptionSender
    {
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly IMessageProtocol[] _messageProtocols;

        public MessageSubscriptionSender(ISubscriptionManager subscriptionManager, IMessageProtocol[] messageProtocols)
        {
            _subscriptionManager = subscriptionManager;
            _messageProtocols = messageProtocols;
        }

        public Task SendMessageToSubscribers<T>(string groupId, T content, CancellationToken cancellationToken)
        {
            var subscribers = _subscriptionManager.GetSubscribers(groupId);

            var aggregatedSendMessageTasks = subscribers.Select(s =>
            {
                var protocolsWithSubscriberPresence = _messageProtocols.Where(mp => mp.ContainsUser(s));
                var sendMessageTasks = protocolsWithSubscriberPresence.Select(p => p.SendAsync(s, content, cancellationToken));

                return sendMessageTasks;
            });

            return Task.WhenAll(aggregatedSendMessageTasks.SelectMany(t => t));
        }

        public void SubscribeUserToGroup(string groupId, string connectionId)
        {
            _subscriptionManager.Subscribe(groupId, connectionId);
        }

        public void UnsubscribeUserFromGroup(string groupId, string connectionId)
        {
            _subscriptionManager.Unsubscribe(groupId, connectionId);
        }
    }
}
