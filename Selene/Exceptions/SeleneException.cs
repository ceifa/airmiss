using System;
using System.Text.Json.Serialization;
using Selene.Messaging;

namespace Selene.Exceptions
{
    public class SeleneException : Exception
    {
        public int ReasonCode { get; }

        public SeleneException(int reasonCode, string message) : base(message)
        {
            ReasonCode = reasonCode;
        }

        public SeleneException(Reason reasonCode, string message) : this((int)reasonCode, message)
        {
        }

        public SeleneException(int reasonCode, string message, Exception innerException) : base(message, innerException)
        {
            ReasonCode = reasonCode;
        }

        public string SerializeJson()
        {
            return JsonSerializer.ToString(new
            {
                ReasonCode,
                Message
            }, new JsonSerializerOptions()
            {
                IgnoreNullValues = true
            });
        }
    }
}
