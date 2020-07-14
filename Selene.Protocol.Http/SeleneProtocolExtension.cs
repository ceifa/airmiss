using Selene.Configuration;
using Selene.Protocol.Http.Listener;
using System;

namespace Selene.Protocol.Http
{
    public static class SeleneProtocolExtension
    {
        public static SeleneConfiguration Http(
            this ProtocolConfiguration protocolConfiguration,
            params string[] addresses)
        {
            if (protocolConfiguration is null)
            {
                throw new ArgumentNullException(nameof(protocolConfiguration));
            }

            var httpListener = new DefaultHttpListener(addresses);
            var httpProtocol = new HttpProtocol(httpListener);

            return protocolConfiguration.Add(httpProtocol);
        }
    }
}
