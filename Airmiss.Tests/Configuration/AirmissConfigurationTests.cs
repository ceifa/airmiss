using System;
using Xunit;

namespace Airmiss.Tests.Configuration
{
    public class AirmissConfigurationTests
    {
        [Fact]
        public void AirmissRunnerShouldBeCreated()
        {
            var AirmissConfiguration = new AirmissConfiguration();

            var runner = AirmissConfiguration.GetRunner();

            Assert.NotNull(runner);
        }

        [Fact]
        public void AirmissRunnerShouldNotReferenceToItsConfigurationAfterBeingCreated()
        {
            (AirmissRunner, WeakReference) CreateRunner()
            {
                var AirmissConfiguration = new AirmissConfiguration();
                return (AirmissConfiguration.GetRunner(), new WeakReference(AirmissConfiguration));
            }
            var (runner, wr) = CreateRunner();

#pragma warning disable S1215 // "GC.Collect" should not be called
            GC.Collect();
#pragma warning restore S1215 // "GC.Collect" should not be called

            Assert.False(wr.IsAlive);
            GC.KeepAlive(runner);
        }
    }
}
