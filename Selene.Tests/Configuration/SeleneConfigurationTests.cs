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
    }
}
