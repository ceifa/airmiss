using System.Threading;
using System.Threading.Tasks;

namespace Selene.Processor
{
    internal interface IMessageSubscriptionSender
    {
        void SubscribeUserToGroup(string groupId, string connectionId);

        void UnsubscribeUserFromGroup(string groupId, string connectionId);

        Task SendMessageToSubscribers<T>(string groupId, T content, CancellationToken cancellationToken);
    }
}
