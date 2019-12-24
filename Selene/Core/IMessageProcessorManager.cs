﻿using System.Threading;
using System.Threading.Tasks;
using Selene.Messaging;

namespace Selene.Core
{
    public interface IMessageProcessorManager
    {
        Task<T> ProcessAsync<T>(MessageReceiver receiver, Message message, CancellationToken cancellationToken);
    }
}