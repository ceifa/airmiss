using System;

namespace Selene.Internal.Processor.Hub
{
    public interface IHubLifecycle
    {
        object Hub { get; }

        Action HubReleaser { get; }
    }
}
