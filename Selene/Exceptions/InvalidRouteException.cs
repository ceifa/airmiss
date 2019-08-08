using System;
using System.Collections.Generic;
using System.Text;

namespace Selene.Exceptions
{
    public class InvalidRouteException : Exception
    {
        public InvalidRouteException()
        {
        }

        public InvalidRouteException(string message) : base(message)
        {
        }

        public InvalidRouteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
