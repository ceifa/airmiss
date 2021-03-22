using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Airmiss.Core;
using Airmiss.Internal.Processor;
using Airmiss.Messaging;
using Airmiss.Processor;

namespace Airmiss.Internal.Client
{
    internal class ContextProvider : IContextProvider
    {
        public IContext GetContext(IClient client, ProcessorContext processorContext, Message message,
            CancellationToken cancellationToken)
        {
            var expectedParameters = processorContext.ProcessorDescriptor.ProcessorMethod.GetParameters();
            var arguments = expectedParameters.Select(ep =>
            {
                var type = ep.ParameterType;

                var pathParameterName = ep.GetCustomAttribute<PathAttribute>()?.Name ?? ep.Name;
                if (pathParameterName is not null && processorContext.UriParametersArguments.TryGetValue(pathParameterName.ToLowerInvariant(), out var value))
                    return Convert.ChangeType(value, type);

                if (type.IsAssignableFrom(typeof(CancellationToken)))
                    return cancellationToken;

                if (ep.GetCustomAttribute<ContentAttribute>() != null)
                    return Convert.ChangeType(message.Content, type);

                return type.IsValueType ? Activator.CreateInstance(type) : default;
            }).ToArray();

            return new ClientContext(client, processorContext.ProcessorDescriptor, arguments);
        }
    }
}