using System;
using Xunit;

namespace Selene.Tests.Configuration
{
    public class SeleneConfigurationTests
    {
        [Fact]
        public void SeleneShouldNotReferenceToItsConfigurationAfterBeingCreated()
        {
            var seleneConfiguration = new SeleneConfiguration();
            var wr = new WeakReference(seleneConfiguration);
            var selene = seleneConfiguration.GetRunner();

#pragma warning disable
            GC.Collect();
#pragma warning restore

            Assert.False(wr.IsAlive);
            GC.KeepAlive(selene);
        }
    }
}
