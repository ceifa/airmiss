using Xunit;

namespace Selene.Tests.Configuration
{
    public class SeleneConfigurationTests
    {
        [Fact]
        public void SeleneRunnerShouldBeCreatedSuccessfully()
        {
            var seleneConfiguration = new SeleneConfiguration();

            var runner = seleneConfiguration.GetRunner();

            Assert.NotNull(runner);
        }
    }
}
