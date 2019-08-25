using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Selene.Internal.Providers;
using Selene.Messaging;

namespace Selene.Processor
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IMessageProcessorDescriptorProvider _messageProcessorDescriptorProvider;
        private readonly INodeConnectionManager _nodeConnectionManager;
        private readonly ITypeActivatorCache _typeActivatorCache;

        internal MessageProcessor(IMessageProcessorDescriptorProvider messageProcessorDescriptorProvider,
            ITypeActivatorCache typeActivatorCache, INodeConnectionManager nodeConnectionManager)
        {
            _messageProcessorDescriptorProvider = messageProcessorDescriptorProvider ??
                                                  throw new ArgumentNullException(
                                                      nameof(messageProcessorDescriptorProvider));
            _typeActivatorCache = typeActivatorCache ?? throw new ArgumentNullException(nameof(typeActivatorCache));
            _nodeConnectionManager =
                nodeConnectionManager ?? throw new ArgumentNullException(nameof(nodeConnectionManager));
        }

        public async Task<T> ProcessAsync<T>(string connectionId, Message message,
            CancellationToken cancellationToken)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var descriptor = _messageProcessorDescriptorProvider.GetDescriptor(new Route(message.Route), message.Verb);
            var hubInstance = _typeActivatorCache.GetInstance(descriptor.MessageProcessorDescriptor.HubType);
            var connectionContext = _nodeConnectionManager.GetConnectionContext(connectionId);

            try
            {
                if (hubInstance is MessageProcessorHub messageProcessorHub)
                    messageProcessorHub.Context = connectionContext;

                var messageProcessorMethod = descriptor.MessageProcessorDescriptor.MessageProcessor;
                var parameters = GetParameters(messageProcessorMethod.GetParameters(), descriptor.Variables,
                    message.Content,
                    cancellationToken);
                var messageProcessorResult = messageProcessorMethod.Invoke(hubInstance, parameters.ToArray());

                switch (messageProcessorResult)
                {
                    case Task<T> genericTask:
                        return await genericTask;
                    case Task task:
                        await task;
                        return default;
                    case T resultObject:
                        return resultObject;
                    default:
                        throw new InvalidCastException(
                            $"Value returned from message processor is not of type {typeof(T).Name}");
                }
            }
            finally
            {
                _typeActivatorCache.Release(hubInstance);
                _nodeConnectionManager.SetConnectionContext(connectionContext);
            }
        }

        private IEnumerable<object> GetParameters(IEnumerable<ParameterInfo> expectedParameters,
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