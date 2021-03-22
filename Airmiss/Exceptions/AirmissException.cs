using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Airmiss.Exceptions
{
#pragma warning disable
    public class AirmissException : Exception
#pragma warning restore
    {
        public AirmissException(int code, string message) : base(message)
        {
            Code = code;
        }

        public AirmissException(int code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }

        public int Code { get; }

        public byte[] SerializeUtf8()
        {
            return JsonSerializer.SerializeToUtf8Bytes(new
            {
                Code,
                Message
            });
        }

        public Task SerializeJsonAsync(Stream writableStream)
        {
            return JsonSerializer.SerializeAsync(writableStream, new
            {
                Code,
                Message
            });
        }
    }
}