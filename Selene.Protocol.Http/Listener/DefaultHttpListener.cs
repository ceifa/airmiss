using Selene.Core;
using Selene.Exceptions;
using Selene.Messaging;
using Selene.Processor;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Selene.Protocol.Http.Listener
{
    internal class DefaultHttpListener : IHttpListener
    {
        private readonly HttpListener _httpListener;

        public DefaultHttpListener(string[] addresses)
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

                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                }
                catch (Exception ex)
                {
                    var seleneException = ex as SeleneException ?? new SeleneException(500, ex.Message);

                    await seleneException.SerializeJsonAsync(context.Response.OutputStream);
                    context.Response.StatusCode = seleneException.Code;
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

        private async Task HandleResultAsync(ProcessorResult result, HttpListenerContext context, CancellationToken cancellationToken)
        {
            if (Convert.GetTypeCode(result.Result) == TypeCode.Object)
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                await JsonSerializer.SerializeAsync(context.Response.OutputStream, result.Result, result.Type, default, cancellationToken);
            }
            else
            {
                var buffer = Encoding.UTF8.GetBytes(result.Result.ToString());

                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.OutputStream.WriteAsync(buffer, cancellationToken);
            }
        }

        private async Task<Message> GetMessageAsync(HttpListenerContext context, CancellationToken cancellationToken)
        {
            return new Message
            {
                Verb = context.Request.HttpMethod switch
                {
                    "GET" => Verb.Get,
                    "POST" => Verb.Post,
                    "PUT" => Verb.Put,
                    "DELETE" => Verb.Delete,
                    "PATCH" => Verb.Patch,
                    _ => throw new InvalidOperationException($"Method {context.Request.HttpMethod} does not exists in HTTP protocol")
                },

                Content = context.Request.HasEntityBody ? await JsonSerializer.DeserializeAsync<object>(
                    context.Request.InputStream, cancellationToken: cancellationToken) : default,

                Route = context.Request.Url
            };
        }
    }
}
