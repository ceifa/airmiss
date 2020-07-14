using System;
using System.Threading.Tasks;
using Selene.Protocol.Http;

namespace Selene.TestConsole
{
    public static class Program
    {
        internal static async Task Main(string[] args)
        {
            var runner = new SeleneConfiguration()
                .Processor.AddCurrentAssembly()
                .Protocol.Http("http://localhost:1337/")
                .GetRunner();

            await runner.StartAsync();

            Console.WriteLine("Started");
            Console.ReadKey();
        }
    }
}
