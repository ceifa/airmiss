﻿using Selene.Messaging;
using Selene.Processor;
using Selene.Tests.Protocol;
using System.Threading.Tasks;
using Xunit;

namespace Selene.Tests.Processor
{
    public class ProcessorRoutingTests
    {
        [Fact]
        public async Task ShouldFindTheProcessor()
        {
            var protocol = new DummyProtocol();
            var runner = new SeleneConfiguration()
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
            var runner = new SeleneConfiguration()
                .Processor.Add(typeof(DummyHub), typeof(DummyHub).GetMethod(nameof(DummyHub.Dummy)))
                .Protocol.Add(protocol)
                .GetRunner();
            await runner.StartAsync();

            var quantity = 3;
            int actual = 0;

            for (int i = 0; i < quantity; i++)
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
            var runner = new SeleneConfiguration()
                .Processor.AddHub<DummyHub>()
                .Protocol.Add(protocol)
                .GetRunner();
            await runner.StartAsync();

            var result = await protocol.ProcessAsync<string>(new Message
            {
                Route = nameof(DummyHub.Dummy2),
                Verb = Verb.Get
            });

            Assert.Equal(DummyHub.Dummy2Return, result);
        }
    }
}