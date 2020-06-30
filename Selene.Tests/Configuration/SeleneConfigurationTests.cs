using Selene.Tests.Protocol;
using System;
using Xunit;

namespace Selene.Tests.Configuration
{
    public class SeleneConfigurationTests
    {
        [Fact]
        public void SeleneRunnerShouldBeCreated()
        {
            var seleneConfiguration = new SeleneConfiguration();

            var runner = seleneConfiguration.GetRunner();

            Assert.NotNull(runner);
        }

        [Fact]
        public void SeleneRunnerShouldNotReferenceToItsConfigurationAfterBeingCreated()
        {
            (SeleneRunner, WeakReference) CreateRunner()
            {
                var seleneConfiguration = new SeleneConfiguration();
                return (seleneConfiguration.GetRunner(), new WeakReference(seleneConfiguration));
            }
            var (runner, wr) = CreateRunner();

            GC.Collect();

            Assert.False(wr.IsAlive);
            GC.KeepAlive(runner);
        }

        [Fact]
        public void SeleneRunnerShouldDisposeProtocolAfterBeingDisposed()
        {
            var disposableProtocol = new DisposableProtocol();
            var runner = new SeleneConfiguration()
                .Protocol.Add(disposableProtocol)
                .GetRunner();
            
            runner.Dispose();

            Assert.True(disposableProtocol.IsDisposable);
        }
    }
}
