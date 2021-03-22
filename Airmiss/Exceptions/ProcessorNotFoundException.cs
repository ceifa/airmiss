using System;

namespace Airmiss.Exceptions
{
#pragma warning disable
    public class ProcessorNotFoundException : Exception
#pragma warning restore
    {
        internal ProcessorNotFoundException()
        {
        }

        internal ProcessorNotFoundException(string message) : base(message)
        {
        }

        internal ProcessorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}