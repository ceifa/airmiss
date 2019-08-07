using Selene.Messaging;
using Selene.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Processor
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IMessageProcessorDescriptorProvider _messageProcessorDescriptorProvider;
        private readonly ITypeActivatorCache _typeActivatorCache;
        private readonly INodeConnectionManager _nodeConnectionManager;

        internal MessageProcessor(IMessageProcessorDescriptorProvider messageProcessorDescriptorProvider,
            ITypeActivatorCache typeActivatorCache, INodeConnectionManager nodeConnectionManager)
        {
            _messageProcessorDescriptorProvider = messageProcessorDescriptorProvider ??
                                                  throw new ArgumentNullException(
                                                      nameof(messageProcessorDescriptorProvider));
            _typeActivatorCache = typeActivatorCache ?? throw new ArgumentNullException(nameof(typeActivatorCache));
            _nodeConnectionManager = nodeConnectionManager ?? throw new ArgumentNullException(nameof(nodeConnectionManager));
        }

        public async Task<object> ProcessAsync(string connectionId, Message message,
            CancellationToken cancellationToken)
        {
            var descriptor = _messageProcessorDescriptorProvider.GetDescriptor(message.Route, message.Verb);
            var hubInstance = _typeActivatorCache.GetInstance(descriptor.MessageProcessorDescriptor.HubType);
            var connectionContext = _nodeConnectionManager.GetConnectionContext(connectionId);

            try
            {
                if (hubInstance is MessageProcessorHub messageProcessorHub)
                {
                    messageProcessorHub.Context = connectionContext;
                }

                var messageProcessorMethod = descriptor.MessageProcessorDescriptor.MessageProcessor;
                var parameters = GetParameters(messageProcessorMethod.GetParameters(), descriptor.Variables, message.Content,
                    cancellationToken);
                var messageProcessorResult = messageProcessorMethod.Invoke(hubInstance, parameters.ToArray());

                switch (messageProcessorResult)
                {
                    case Task<object> genericTask:
                        return await genericTask;
                    case Task task:
                        await task;
                        return null;
                    default:
                        return messageProcessorResult;
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
                var pathParameterName = e.GetCustomAttribute<PathAttribute>()?.Name ?? e.Name;
                if (routeParameters.TryGetValue(pathParameterName.ToLowerInvariant(), out var value)) return value;

                if (e.ParameterType.IsAssignableFrom(typeof(CancellationToken))) return cancellationToken;

                if (e.GetCustomAttribute<ContentAttribute>() != null || e.Name == nameof(content)) return content;

                return default;
            });
        }
    }
}