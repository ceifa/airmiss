using Airmiss.Core;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Protocol.Http.Listener
{
    internal class DefaultTcpListener : ITcpListener
    {
        private readonly TcpListener _tcpListener;

        public DefaultTcpListener(string address, int port)
        {
            _tcpListener = new TcpListener(IPAddress.Parse(address), port);
        }

        public async Task Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            _tcpListener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var context = await _tcpListener.AcceptTcpClientAsync(cancellationToken);
                var client = new TcpClient(Guid.NewGuid().ToString(), context);

                var thread = new Thread(async () => await HandleClient(client, messageProcessor, cancellationToken));
                thread.Start();
            }
        }

        public Task Stop(CancellationToken cancellationToken)
        {
            _tcpListener.Stop();
            return Task.CompletedTask;
        }

        private static async Task HandleClient(TcpClient tcpClient, IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];
            // TODO: Do a proper buffer handling
            var currentMessage = new StringBuilder();
            int brackets = 0;

            var bracketsBytes = Encoding.UTF8.GetBytes("{}");
            var openBracket = bracketsBytes[0];
            var closeBracket = bracketsBytes[1];

            var stream = tcpClient.Tcp.GetStream();

            while (tcpClient.Tcp.Connected)
            {
                await Task.Delay(1);

                if (stream.DataAvailable)
                {
                    var readen = await stream.ReadAsync(buffer, 0, buffer.Length);
                    currentMessage.Append(Encoding.UTF8.GetString(buffer));

                    brackets += buffer.Count(c => c == openBracket);
                    brackets -= buffer.Count(c => c == closeBracket);

                    if (brackets == 0)
                    {
                        var result = await HandleMessage(currentMessage);
                        await stream.WriteAsync(Encoding.UTF8.GetBytes(result));

                        currentMessage.Clear();
                    }
                }
            }
        }
    }
}