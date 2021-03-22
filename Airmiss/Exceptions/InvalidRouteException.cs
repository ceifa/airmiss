using System;
using Airmiss.Processor;

namespace Airmiss.Exceptions
{
#pragma warning disable
    public class InvalidRouteException : Exception
#pragma warning restore
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