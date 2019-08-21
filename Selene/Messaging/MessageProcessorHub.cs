using System.Threading;
using System.Threading.Tasks;

namespace Selene.Messaging
{
    public class MessageProcessorHub
    {
        private static AsyncLocal<ConnectionContext> _currentConnectionContext = new AsyncLocal<ConnectionContext>();

        internal ConnectionContext Context
        {
            get => _currentConnectionContext?.Value;
            set
            {
                if (value != null)
                {
                    _currentConnectionContext.Value = value;
                }
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
