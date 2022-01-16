using System;
using Airmiss.Configuration;
using Airmiss.Protocol.Tcp.Listener;

namespace Airmiss.Protocol.Tcp
{
    public static class AirmissProtocolExtension
    {
        public static AirmissConfiguration Tcp(
            this ProtocolConfiguration protocolConfiguration,
            string address,
            int port)
        {
            if (protocolConfiguration is null) throw new ArgumentNullException(nameof(protocolConfiguration));

            var httpListener = new DefaultTcpListener(address, port);
            var httpProtocol = new TcpProtocol(httpListener);

            return protocolConfiguration.Add(httpProtocol);
        }
    }
}