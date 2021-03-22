using System;
using Airmiss.Configuration;
using Airmiss.Protocol.Http.Listener;

namespace Airmiss.Protocol.Http
{
    public static class AirmissProtocolExtension
    {
        public static AirmissConfiguration Http(
            this ProtocolConfiguration protocolConfiguration,
            params string[] addresses)
        {
            if (protocolConfiguration is null) throw new ArgumentNullException(nameof(protocolConfiguration));

            var httpListener = new DefaultHttpListener(addresses);
            var httpProtocol = new HttpProtocol(httpListener);

            return protocolConfiguration.Add(httpProtocol);
        }
    }
}