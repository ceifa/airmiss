﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;

namespace Airmiss.Protocol.Tcp.Listener
{
    internal interface ITcpListener : IDisposable
    {
        Task Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken);

        Task Stop(CancellationToken cancellationToken);
    }
}