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

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        protected async Task SubscribeAsync(string key)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        protected async Task UnsubscribeAsync(string key)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        protected async Task SendToSubscribersAsync(string key, object content)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
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
