using Selene.Tests.Protocol;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Selene.Tests.Runner
{
    public class SeleneRunnerTests
    {

        [Fact]
        public async Task SeleneRunnerShouldBeRunningAfterStart()
        {
            var runner = new SeleneConfiguration()
                .GetRunner();

            await runner.StartAsync();

            Assert.True(runner.IsRunning);
        }

        [Fact]
        public async Task SeleneRunnerShouldNotBeRunningAfterStop()
        {
            var runner = new SeleneConfiguration()
                .GetRunner();

            await runner.StartAsync();
            await runner.StopAsync();

            Assert.False(runner.IsRunning);
        }

        [Fact]
        public async Task SeleneRunnerShouldNotBeAbleToStopIfNotRunning()
        {
            var runner = new SeleneConfiguration()
                .GetRunner();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await runner.StopAsync();
            });
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await runner.StartAsync();
                await runner.StopAsync();
                await runner.StopAsync();
            });
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

        [Fact]
        public async Task SeleneRunnerShouldNotDisposeWhileRunning()
        {
            var runner = new SeleneConfiguration()
                .GetRunner();

            await runner.StartAsync();

            Assert.Throws<InvalidOperationException>(() =>
            {
                runner.Dispose();
            });
        }

        [Fact]
        public async Task SeleneRunnerShouldNotBeAbleToDisposeAfterStopped()
        {
            var runner = new SeleneConfiguration()
                .GetRunner();

            await runner.StartAsync();
            await runner.StopAsync();

            runner.Dispose();
        }
    }
}
