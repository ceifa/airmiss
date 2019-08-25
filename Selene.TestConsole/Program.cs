using System;
using System.Threading.Tasks;
using Selene.Configuration;
using Selene.Messaging;
using Selene.Protocol.Websocket;

namespace Selene.TestConsole
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var runner = new SeleneConfiguration()
                .Protocol.WebSocket("ws://127.0.0.1:1337")
                .Processor.AddHub<TesteHub>()
                .GetRunner();

            await runner.StartAsync();

            Console.WriteLine("Started. Press enter to stop.");
            Console.Read();
        }
    }

    [MessageProcessorHub("teste")]
    internal class TesteHub
    {
        [MessageProcessor("eae", Verb.Get)]
        public object Eae()
        {
            return 1;
        }
    }
}