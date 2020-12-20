using System;

namespace Airmiss.Internal.Processor.Hub
{
    public interface IHubLifecycle
    {
        object Hub { get; }

        Action HubReleaser { get; }
    }
}