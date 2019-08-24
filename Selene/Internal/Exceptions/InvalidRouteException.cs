using System;
using System.Collections.Generic;
using System.Text;

namespace Selene.Internal.Exceptions
{
    internal class InvalidRouteException : Exception
    {
        private static string GetErrorMessage(string route) => $"Route '{route}' is not a valid route";

        private static string GetErrorMessage(string route, string message) => $"{GetErrorMessage(route)}: {message}";

        internal InvalidRouteException(string route) : base(GetErrorMessage(route))
        {
        }

        internal InvalidRouteException(string route, string message) : base(GetErrorMessage(route, message))
        {
        }

        internal InvalidRouteException(string route, string message, Exception innerException) : base(GetErrorMessage(route, message), innerException)
        {
        }
    }
}
