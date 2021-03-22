using System.Threading.Tasks;
using Airmiss.Messaging;
using Airmiss.Processor;
using Airmiss.Tests.Protocol;
using Xunit;

namespace Airmiss.Tests.Processor
{
    public class ProcessorRoutingTests
    {
        [Fact]
        public async Task ShouldFindTheProcessor()
        {
            var protocol = new DummyProtocol();
            var runner = new AirmissConfiguration()
                .Processor.Add(typeof(DummyHub), typeof(DummyHub).GetMethod(nameof(DummyHub.Dummy)))
                .Protocol.Add(protocol)
                .GetRunner();
            await runner.StartAsync();

            var called = await protocol.ProcessAsync<object>(new Message
            {
                Route = nameof(DummyHub.Dummy),
                Verb = Verb.Get
            });

            Assert.Equal(1, called);
        }

        [Fact]
        public async Task AProcessorCanBeCalledMultipleTimes()
        {
            var protocol = new DummyProtocol();
            var runner = new AirmissConfiguration()
                .Processor.Add(typeof(DummyHub), typeof(DummyHub).GetMethod(nameof(DummyHub.Dummy)))
                .Protocol.Add(protocol)
                .GetRunner();
            await runner.StartAsync();

            const int quantity = 3;
            var actual = 0;

            for (var i = 0; i < quantity; i++)
            {
                actual = await protocol.ProcessAsync<int>(new Message
                {
                    Route = nameof(DummyHub.Dummy),
                    Verb = Verb.Get
                });
            }

            Assert.Equal(quantity, actual);
        }

        [Fact]
        public async Task ShouldFindTheRightProcessor()
        {
            var protocol = new DummyProtocol();
            var runner = new AirmissConfiguration()
                .Processor.AddHub<DummyHub>()
                .Protocol.Add(protocol)
                .GetRunner();
            await runner.StartAsync();

            var result = await protocol.ProcessAsync<string>(new Message
            {
                Route = nameof(DummyHub.Dummy2),
                Verb = Verb.Get
            });

            Assert.Equal(DummyHub.DummyReturn, result);
        }

        [Fact]
        public async Task ShouldAwaitAnAsyncProcessor()
        {
            var protocol = new DummyProtocol();
            var runner = new AirmissConfiguration()
                .Processor.AddHub<DummyHub>()
                .Protocol.Add(protocol)
                .GetRunner();
            await runner.StartAsync();

            var result = await protocol.ProcessAsync<string>(new Message
            {
                Route = nameof(DummyHub.DummyAsync),
                Verb = Verb.Get
            });

            Assert.Equal(DummyHub.DummyReturn, result);
        }
    }
}