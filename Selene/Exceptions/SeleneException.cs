using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Selene.Exceptions
{
    public class SeleneException : Exception
    {
        public int Code { get; }

        public SeleneException(int code, string message) : base(message)
        {
            Code = code;
        }

        public SeleneException(int code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }

        public string SerializeJson()
        {
            return JsonSerializer.Serialize(new
            {
                Code,
                Message
            }, new JsonSerializerOptions()
            {
                IgnoreNullValues = true
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
