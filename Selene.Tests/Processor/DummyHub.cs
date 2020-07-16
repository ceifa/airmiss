using Selene.Processor;
using System.Threading.Tasks;

namespace Selene.Tests.Processor
{
    public class DummyHub
    {
        public static string DummyReturn => "Hi";

        public int Called { get; private set; }

        [Processor(nameof(Dummy), Verb.Get)]
        public int Dummy()
        {
            return ++Called;
        }

        [Processor(nameof(Dummy2), Verb.Get)]
        public string Dummy2()
        {
            return DummyReturn;
        }

        [Processor(nameof(DummyAsync), Verb.Get)]
        public async Task<string> DummyAsync()
        {
            await Task.Delay(1000);
            return DummyReturn;
        }
    }
}
