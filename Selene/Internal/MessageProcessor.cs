using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Selene.Core;
using Selene.Messaging;
using Selene.Processor;

namespace Selene.Internal
{
    internal class MessageProcessor : IMessageProcessor
    {
        private readonly IMessageProcessorProvider _messageProcessorProvider;
        private readonly IReceiverContextManager _receiverContextManager;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly ITypeActivatorCache _typeActivatorCache;

        internal MessageProcessor(
            IMessageProcessorProvider messageProcessorProvider,
            ITypeActivatorCache typeActivatorCache,
            IReceiverContextManager receiverContextManager,
            ISubscriptionManager subscriptionManager)
        {
            _messageProcessorProvider = messageProcessorProvider;
            _typeActivatorCache = typeActivatorCache;
            _receiverContextManager = receiverContextManager;
            _subscriptionManager = subscriptionManager;
        }

        public async Task<T> ProcessAsync<T>(MessageReceiver receiver, Message message, CancellationToken cancellationToken)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var processor = _messageProcessorProvider.GetProcessor(message.Route, message.Verb);
            var hubInstance = _typeActivatorCache.GetInstance(processor.MessageProcessor.HubType);
            var context = _receiverContextManager.GetContext(receiver);

            try
            {
                PopulateMessageProcessorVariables(hubInstance, context);

                var messageProcessorMethod = processor.MessageProcessor.ProcessorMethod;
                var parameters = GetProcessorParameters(messageProcessorMethod.GetParameters(), processor.Variables,
                    message.Content, cancellationToken);

                var messageProcessorResult = messageProcessorMethod.Invoke(hubInstance, parameters.ToArray());

                return messageProcessorResult switch
                {
                    Task<T> genericTask => await genericTask,
                    Task task => await task.ContinueWith(_ => default(T)),
                    T resultObject => resultObject,
                    _ => throw new InvalidCastException(
                        $"Value returned from message processor is not of type {typeof(T).Name}")
                };
            }
            finally
            {
                _typeActivatorCache.Release(hubInstance);
                _receiverContextManager.SetContext(context);
            }
        }

        private void PopulateMessageProcessorVariables(object instance, ReceiverContext context)
        {
            if (instance is MessageProcessorHub messageProcessorHub)
            {
                messageProcessorHub.Context = context;
                messageProcessorHub.SubscriptionManager = _subscriptionManager;
            }
        }

        private IEnumerable<object> GetProcessorParameters(IEnumerable<ParameterInfo> expectedParameters,
            IDictionary<string, string> routeParameters, object content, CancellationToken cancellationToken)
        {
            return expectedParameters.Select(e =>
            {
                var type = e.ParameterType;

                var pathParameterName = e.GetCustomAttribute<PathAttribute>()?.Name ?? e.Name;
                if (routeParameters.TryGetValue(pathParameterName.ToLowerInvariant(), out var value))
                    return Convert.ChangeType(value, type);

                if (type.IsAssignableFrom(typeof(CancellationToken)))
                    return cancellationToken;

                if (e.GetCustomAttribute<ContentAttribute>() != null || e.Name == nameof(content))
                    return Convert.ChangeType(content, type);

                return type.IsValueType ? Activator.CreateInstance(type) : default;
            });
        }
    }
}