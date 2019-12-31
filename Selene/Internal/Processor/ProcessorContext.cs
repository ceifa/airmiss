using System;
using System.Collections.Generic;
using System.Reflection;
using Selene.Internal.Processor.Hub;

namespace Selene.Internal.Processor
{
    internal class ProcessorContext : IDisposable
    {
        private readonly Action _hubReleaser;

        public ProcessorContext(
            IHubLifecycle hubLifecycle,
            MethodInfo processorMethod,
            IDictionary<string, string> parametersArguments)
        {
            _hubReleaser = hubLifecycle.HubReleaser;
            HubInstance = hubLifecycle.Hub;
            ProcessorMethod = processorMethod;
            ParametersArguments = parametersArguments;
        }

        public object HubInstance { get; }

        public MethodInfo ProcessorMethod { get; }

        public IDictionary<string, string> ParametersArguments { get; }

        public void Dispose()
        {
            _hubReleaser();
        }
    }
}