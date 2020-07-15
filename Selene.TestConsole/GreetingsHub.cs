using Selene.Messaging;
using Selene.Processor;
using System.Collections.Generic;
using System.Linq;

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

        [Processor("about", Verb.Get)]
        public object About()
        {
            return new
            {
                Name = "John"
            };
        }

        [Processor("hello/{name}", Verb.Post)]
        public IEnumerable<string> HelloWorlds(string[] content)
        {
            return content.Select(c => $"Hello {c}!");
        }
    }
}
