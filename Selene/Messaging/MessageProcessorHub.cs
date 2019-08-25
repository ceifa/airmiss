using System.Threading;
using System.Threading.Tasks;

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

        protected async Task SubscribeAsync(string key)
        {
        }

        protected async Task UnsubscribeAsync(string key)
        {
        }

        protected async Task SendToSubscribersAsync(string key, object content)
        {
        }

        protected virtual Task BeforeCalledAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task AfterCalledAsync()
        {
            return Task.CompletedTask;
        }
    }
}