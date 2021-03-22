using Airmiss.Core;

namespace Airmiss.Protocol.Http
{
    internal class HttpClient : IClient
    {
        public HttpClient(string identity)
        {
            Identity = identity;
        }

        public string Identity { get; }
    }
}