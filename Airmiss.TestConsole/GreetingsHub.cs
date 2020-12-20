using Airmiss.Processor;
using System.Collections.Generic;
using System.Linq;

namespace Airmiss.TestConsole
{
    [ProcessorHub("greetings")]
    public class GreetingsHub
    {
        public class User
        {
            public string Name { get; set; }
        }

        [Processor("hello/{name}", Verb.Get)]
        public string HelloWorld(string name)
        {
            return $"Hello {name}!";
        }

        [Processor("about", Verb.Get)]
        public User About()
        {
            return new User
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
