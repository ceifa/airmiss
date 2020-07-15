using System;
using Selene.Processor;

namespace Selene.Exceptions
{
    public class InvalidRouteException : Exception
    {
        internal InvalidRouteException(string route) : base(GetErrorMessage(route))
        {
        }

        internal InvalidRouteException(Route route) : base(GetErrorMessage(route.ToString()))
        {
        }

        internal InvalidRouteException(string route, string message) : base(GetErrorMessage(route, message))
        {
        }

        internal InvalidRouteException(string route, string message, Exception innerException) : base(
            GetErrorMessage(route, message), innerException)
        {
        }

        private static string GetErrorMessage(string route)
        {
            return $"Route '{route}' is not a valid route";
        }

        private static string GetErrorMessage(string route, string message)
        {
            return $"{GetErrorMessage(route)}: {message}";
        }
    }
}