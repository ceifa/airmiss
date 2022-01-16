using Airmiss.Core;
using Airmiss.Exceptions;
using Airmiss.Messaging;
using System;
using System.Buffers;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Airmiss.Protocol.Tcp.Listener
{
    internal class DefaultTcpListener : ITcpListener
    {
        private const int DEFAULT_BUFFER_SIZE = 8192;
        private const int DEFAULT_MAX_BUFFER_SIZE = DEFAULT_BUFFER_SIZE * 1024;

        private readonly TcpListener _tcpListener;
        private readonly JsonBuffer _jsonBuffer;

        public DefaultTcpListener(string address, int port)
        {
            _tcpListener = new TcpListener(IPAddress.Parse(address), port);
            _jsonBuffer = new JsonBuffer(DEFAULT_BUFFER_SIZE, DEFAULT_MAX_BUFFER_SIZE, ArrayPool<byte>.Shared);
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

        private async Task HandleClient(TcpClient tcpClient, IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            var stream = tcpClient.Tcp.GetStream();

            while (tcpClient.Tcp.Connected && stream.CanRead && !cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1, cancellationToken);

                try
                {
                    var message = GetMessageFromBuffer();

                    if (message == null)
                    {
                        var read = await stream.ReadAsync(
                            _jsonBuffer.Buffer.AsMemory(_jsonBuffer.BufferCurPos, _jsonBuffer.Buffer.Length - _jsonBuffer.BufferCurPos),
                            cancellationToken);

                        if (read == 0)
                        {
                            break;
                        }

                        _jsonBuffer.BufferCurPos += read;

                        if (_jsonBuffer.BufferCurPos >= _jsonBuffer.Buffer.Length)
                        {
                            _jsonBuffer.IncreaseBuffer();
                        }
                    }
                    else
                    {
                        try
                        {
                            var result = await messageProcessor.ProcessAsync(tcpClient, message, cancellationToken);
                            await tcpClient.SendAsync(message.CorrelationId, result.Type, result.Result, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            var airmissException = ex as AirmissException ?? new AirmissException(500, ex.Message);
                            if (stream.CanWrite)
                            {
                                await tcpClient.SendAsync(message.CorrelationId, airmissException, cancellationToken);
                            }
                        }
                    }
                }
                catch (Exception ex) when (ex is IOException || ex is SocketException)
                {
                    break;
                }
            }

            // It calls .Dispose() internally
            tcpClient.Tcp.Close();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Message GetMessageFromBuffer()
        {
            Message message = null;

            if (_jsonBuffer.TryExtractJsonFromBuffer(out var jsonBytes))
            {
                message = JsonSerializer.Deserialize<Message>(jsonBytes, DefaultJsonSerializerOptions.Options);
            }

            return message;
        }

        public void Dispose()
        {
            _jsonBuffer.Dispose();
        }
    }
}