using Selene.Configuration;
using Selene.Messaging;
using Selene.Protocol.Websocket;
using System;
using System.Threading.Tasks;

namespace Selene.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var runner = new SeleneConfiguration()
                .Protocol.WebSocket("ws://127.0.0.1:1337")
                .Processor.AddHub<TesteHub>()
                .GetRunner();

            await runner.StartAsync();

            Console.Read();
        }
    }

    [MessageProcessorHub("teste")]
    class TesteHub
    {
        [MessageProcessor("eae", Verb.Get)]
        public object Eae()
        {
            return 1;
        }
    }
}
