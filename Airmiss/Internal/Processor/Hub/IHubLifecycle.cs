using System;

namespace Airmiss.Internal.Processor.Hub
{
    internal interface IHubLifecycle
    {
        object Hub { get; }

        Action HubReleaser { get; }
    }
}