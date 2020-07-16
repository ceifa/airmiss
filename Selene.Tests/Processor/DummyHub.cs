using Selene.Processor;

namespace Selene.Tests.Processor
{
    public class DummyHub
    {
        public static string Dummy2Return => "Hi";

        public int Called { get; private set; }

        [Processor(nameof(Dummy), Verb.Get)]
        public int Dummy()
        {
            return ++Called;
        }

        [Processor(nameof(Dummy2), Verb.Get)]
        public string Dummy2()
        {
            return Dummy2Return;
        }
    }
}
