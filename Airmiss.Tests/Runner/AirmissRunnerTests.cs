using System;
using System.Threading.Tasks;
using Airmiss.Tests.Protocol;
using Xunit;

namespace Airmiss.Tests.Runner
{
    public class AirmissRunnerTests
    {
        [Fact]
        public async Task AirmissRunnerShouldBeRunningAfterStart()
        {
            var runner = new AirmissConfiguration()
                .GetRunner();

            await runner.StartAsync();

            Assert.True(runner.IsRunning);
        }

        [Fact]
        public async Task AirmissRunnerShouldNotBeRunningAfterStop()
        {
            var runner = new AirmissConfiguration()
                .GetRunner();

            await runner.StartAsync();
            await runner.StopAsync();

            Assert.False(runner.IsRunning);
        }

        [Fact]
        public async Task AirmissRunnerShouldNotBeAbleToStopIfNotRunning()
        {
            var runner = new AirmissConfiguration()
                .GetRunner();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await runner.StopAsync());
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await runner.StartAsync();
                await runner.StopAsync();
                await runner.StopAsync();
            });
        }

        [Fact]
        public void AirmissRunnerShouldDisposeProtocolAfterBeingDisposed()
        {
            var disposableProtocol = new DisposableProtocol();
            var runner = new AirmissConfiguration()
                .Protocol.Add(disposableProtocol)
                .GetRunner();

            runner.Dispose();

            Assert.True(disposableProtocol.IsDisposed);
        }

        [Fact]
        public async Task AirmissRunnerShouldNotDisposeWhileRunning()
        {
            var disposableProtocol = new DisposableProtocol();
            var runner = new AirmissConfiguration()
                .Protocol.Add(disposableProtocol)
                .GetRunner();

            await runner.StartAsync();
            runner.Dispose();

            Assert.True(runner.IsRunning);
            Assert.False(disposableProtocol.IsDisposed);
        }

        [Fact]
        public async Task AirmissRunnerShouldBeAbleToDisposeAfterStopped()
        {
            var runner = new AirmissConfiguration()
                .GetRunner();

            await runner.StartAsync();
            await runner.StopAsync();

            runner.Dispose();
            Assert.False(runner.IsRunning);
        }
    }
}