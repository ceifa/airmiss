using System;
using System.Threading.Tasks;
using Airmiss.Protocol.Http;
using Airmiss.Protocol.Websocket;
using Airmiss.Protocol.Tcp;

namespace Airmiss.TestConsole
{
    public static class Program
    {
#pragma warning disable
        internal static async Task Main(string[] args)
#pragma warning restore
        {
            var runner = new AirmissConfiguration()
                .Processor.AddCurrentAssembly()
                .Protocol.Http("http://localhost:1337/")
                .Protocol.Websocket("http://localhost:1338/")
                .Protocol.Tcp("127.0.0.1", 1339)
                .GetRunner();

            await runner.StartAsync().ConfigureAwait(false);

            Console.WriteLine("Started");
            Console.ReadKey();
        }
    }
}