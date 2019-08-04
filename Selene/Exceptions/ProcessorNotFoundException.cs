using System;

namespace Selene.Exceptions
{
#pragma warning disable S3925
    public class ProcessorNotFoundException : Exception
#pragma warning enable S3925
    {
        public ProcessorNotFoundException()
        {
        }

        public ProcessorNotFoundException(string message) : base(message)
        {
        }

        public ProcessorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}