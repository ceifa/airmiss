using Selene.Processor;

namespace Selene.Tests.Processor
{
    public class DummyHub
    {
        public static int Called;

        [Processor(nameof(Dummy), Verb.Get)]
        public int Dummy()
        {
            return Called++;
        }
    }
}
