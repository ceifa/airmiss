using Selene.Messaging;
using Selene.Processor;

namespace Selene.TestConsole
{
    [ProcessorHub("greetings")]
    public class GreetingsHub
    {
        [Processor("hello/{name}", Verb.Get)]
        public string HelloWorld(string name)
        {
            return $"Hello {name}!";
        }
    }
}
