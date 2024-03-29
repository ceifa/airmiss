﻿namespace Airmiss.Core
{
    public interface IContext
    {
        IClient Sender { get; }

        IProcessorDescriptor Processor { get; }

        object?[] Arguments { get; }
    }
}