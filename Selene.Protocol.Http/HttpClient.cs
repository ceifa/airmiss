using Selene.Core;

namespace Selene.Protocol.Http
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
