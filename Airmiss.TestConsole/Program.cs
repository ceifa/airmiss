using System;
using System.Threading.Tasks;
using Airmiss.Protocol.Http;

namespace Airmiss.TestConsole
{
    public static class Program
    {
        internal static async Task Main(string[] args)
        {
            var runner = new AirmissConfiguration()
                .Processor.AddCurrentAssembly()
                .Protocol.Http("http://localhost:1337/")
                .GetRunner();

            await runner.StartAsync();

            Console.WriteLine("Started");
            Console.ReadKey();
        }
    }
}
