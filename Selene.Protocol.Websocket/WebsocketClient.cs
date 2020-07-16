using Selene.Core;

namespace Selene.Protocol.Websocket
{
    public class WebsocketClient : IClient
    {
        public WebsocketClient(string identity)
        {
            Identity = identity;
        }

        public string Identity { get; }
    }
}
