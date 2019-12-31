using System;

namespace Selene.Internal.Processor.Hub
{
    internal class HubLifecycle : IHubLifecycle
    {
        public HubLifecycle(object hub, Action hubReleaser)
        {
            Hub = hub;
            HubReleaser = hubReleaser;
        }

        public object Hub { get; }

        public Action HubReleaser { get; }
    }
}