using System.Threading;
using System.Threading.Tasks;

namespace Selene.Processor
{
    public abstract class MessageProcessorHub
    {
        private static readonly AsyncLocal<ReceiverContext> CurrentReceiverContext =
            new AsyncLocal<ReceiverContext>();

        internal ReceiverContext Context
        {
            get => CurrentReceiverContext.Value;
            set => CurrentReceiverContext.Value = value;
        }

        internal ISubscriptionManager SubscriptionManager { private get; set; }

        protected void LinkReceiver(string receiverIdentity)
        {
            SubscriptionManager.Subscribe(receiverIdentity, Context.MessageReceiver.Identity);
        }

        protected void UnlinkReceiver(string receiverIdentity)
        {
            SubscriptionManager.Unsubscribe(receiverIdentity, Context.MessageReceiver.Identity);
        }

        protected async Task SendToReceiverAsync(object message, string receiverIdentity = null, CancellationToken cancellationToken = default)
        {
            receiverIdentity ??= Context.MessageReceiver.Identity;
        }
    }
}