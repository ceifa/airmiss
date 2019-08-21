using System;
using System.Collections.Generic;
using System.Text;

namespace Selene.Internal.Exceptions
{
    internal class InvalidRouteException : Exception
    {
        internal InvalidRouteException()
        {
        }

        internal InvalidRouteException(string message) : base(message)
        {
        }

        internal InvalidRouteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
