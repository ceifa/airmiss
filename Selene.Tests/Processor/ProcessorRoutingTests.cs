using Selene.Messaging;
using Selene.Processor;
using Selene.Tests.Protocol;
using System.Threading.Tasks;
using Xunit;

namespace Selene.Tests.Processor
{
    public class ProcessorRoutingTests
    {
        [Fact]
        public async Task SeleneShouldFindTheProcessor()
        {
            var protocol = new DummyProtocol();
            var runner = new SeleneConfiguration()
                .Processor.Add(typeof(DummyHub), typeof(DummyHub).GetMethod(nameof(DummyHub.Dummy)))
                .Protocol.Add(protocol)
                .GetRunner();
            await runner.StartAsync();

            await protocol.ProcessAsync<object>(new Message
            {
                Route = nameof(DummyHub.Dummy),
                Verb = Verb.Get
            });

            Assert.Equal(1, DummyHub.Called);
        }
    }
}
