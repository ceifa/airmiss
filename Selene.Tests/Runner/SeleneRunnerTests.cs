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

            Assert.True(disposableProtocol.IsDisposed);
        }

        [Fact]
        public async Task SeleneRunnerShouldNotDisposeWhileRunning()
        {
            var disposableProtocol = new DisposableProtocol();
            var runner = new SeleneConfiguration()
                .Protocol.Add(disposableProtocol)
                .GetRunner();

            await runner.StartAsync();
            runner.Dispose();

            Assert.True(runner.IsRunning);
            Assert.False(disposableProtocol.IsDisposed);
        }

        [Fact]
        public async Task SeleneRunnerShouldBeAbleToDisposeAfterStopped()
        {
            var runner = new SeleneConfiguration()
                .GetRunner();

            await runner.StartAsync();
            await runner.StopAsync();

            runner.Dispose();
            Assert.False(runner.IsRunning);
        }
    }
}
