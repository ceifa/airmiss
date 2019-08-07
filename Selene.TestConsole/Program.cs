using Selene.Configuration;
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
                .GetRunner();

            await runner.StartAsync();

            Console.Read();
        }
    }
}
