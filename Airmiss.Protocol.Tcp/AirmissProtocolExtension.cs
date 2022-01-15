using System;
using Airmiss.Configuration;
using Airmiss.Protocol.Http.Listener;

namespace Airmiss.Protocol.Tcp
{
    public static class AirmissProtocolExtension
    {
        public static AirmissConfiguration Tcp(
            this ProtocolConfiguration protocolConfiguration,
            params string[] addresses)
        {
            if (protocolConfiguration is null) throw new ArgumentNullException(nameof(protocolConfiguration));

            var httpListener = new DefaultTcpListener(addresses);
            var httpProtocol = new HttpProtocol(httpListener);

            return protocolConfiguration.Add(httpProtocol);
        }
    }
}