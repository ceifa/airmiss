using System;
using System.Collections.Generic;
using Selene.Core;
using Selene.Internal.Processor.Hub;

namespace Selene.Internal.Processor
{
    internal class ProcessorContext : IDisposable
    {
        private readonly Action _hubReleaser;

        public ProcessorContext(
            IHubLifecycle hubLifecycle,
            IProcessorDescriptor processorDescriptor,
            IDictionary<string, string> pathParametersArguments)
        {
            _hubReleaser = hubLifecycle.HubReleaser;
            HubInstance = hubLifecycle.Hub;
            ProcessorDescriptor = processorDescriptor;
            UriParametersArguments = pathParametersArguments;
        }

        public object HubInstance { get; }

        public IProcessorDescriptor ProcessorDescriptor { get; }

        public IDictionary<string, string> UriParametersArguments { get; }

        public void Dispose()
        {
            _hubReleaser();
        }
    }
}