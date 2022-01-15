using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Airmiss.Core;
using Airmiss.Exceptions;
using Airmiss.Messaging;
using Airmiss.Processor;

namespace Airmiss.Protocol.Http.Listener
{
    internal class DefaultHttpListener : IHttpListener
    {
        private readonly HttpListener _httpListener;

        public DefaultHttpListener(IEnumerable<string> addresses)
        {
            _httpListener = new HttpListener();

            foreach (var address in addresses)
                _httpListener.Prefixes.Add(address);
        }

        public async Task Start(IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            _httpListener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var context = await _httpListener.GetContextAsync();
                var client = new HttpClient(default);

                try
                {
                    context.Response.ContentEncoding = Encoding.UTF8;

                    var message = await GetMessageAsync(context, cancellationToken);
                    var result = await messageProcessor.ProcessAsync(client, message, cancellationToken);
                    if (!result.IsEmpty)
                    {
                        await HandleResultAsync(result, context, cancellationToken);

                        context.Response.StatusCode = (int) HttpStatusCode.OK;
                    }
                }
                catch (Exception ex)
                {
                    var AirmissException = ex as AirmissException ?? new AirmissException(500, ex.Message);

                    await AirmissException.SerializeJsonAsync(context.Response.OutputStream);
                    context.Response.StatusCode = AirmissException.Code;
                }
                finally
                {
                    context.Response.Close();
                }
            }
        }

        public Task Stop(CancellationToken cancellationToken)
        {
            _httpListener.Stop();
            return Task.CompletedTask;
        }

        private static async Task HandleResultAsync(ProcessorResult result, HttpListenerContext context,
            CancellationToken cancellationToken)
        {
            if (Convert.GetTypeCode(result.Result) == TypeCode.Object)
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                await JsonSerializer.SerializeAsync(context.Response.OutputStream, result.Result, result.Type, options: default,
                    cancellationToken);
            }
            else
            {
                var buffer = Encoding.UTF8.GetBytes(result.Result.ToString());

                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.OutputStream.WriteAsync(buffer, cancellationToken);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async Task<Message> GetMessageAsync(HttpListenerContext context,
            CancellationToken cancellationToken)
        {
            return new()
            {
                Verb = context.Request.HttpMethod switch
                {
                    "GET" => Verb.Get,
                    "POST" => Verb.Post,
                    "PUT" => Verb.Put,
                    "DELETE" => Verb.Delete,
                    "PATCH" => Verb.Patch,
                    _ => throw new InvalidOperationException(
                        $"Method {context.Request.HttpMethod} does not exists in HTTP protocol")
                },

                Content = context.Request.HasEntityBody
                    ? await JsonSerializer.DeserializeAsync<object>(
                        context.Request.InputStream, cancellationToken: cancellationToken)
                    : default,

                Route = context.Request.Url
            };
        }
    }
}