using System;

namespace Selene.Internal.Exceptions
{
    internal class ProcessorNotFoundException : Exception
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