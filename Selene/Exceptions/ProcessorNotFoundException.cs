using System;

namespace Selene.Exceptions
{
    public class ProcessorNotFoundException : Exception
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