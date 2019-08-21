using Selene.Internal.Exceptions;
using Selene.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Selene.Internal
{
    public class Route
    {
        private const char RouteSeparator = '/';
        private static readonly Regex RouteRepetitionsRegex = new Regex($"{Regex.Escape(RouteSeparator.ToString())}+", RegexOptions.Compiled);

        public string[] RouteTokens => _routeTokens.Value;

        private readonly string _route;
        private readonly Lazy<string[]> _routeTokens;

        public Route(string route)
        {
            _route = route;
            _routeTokens = new Lazy<string[]>(() => GetRouteTokens(SanitizeRoute(route)));
        }

        private static string SanitizeRoute(string route)
        {
            return RouteRepetitionsRegex.Replace(route
                .Trim(RouteSeparator)
                .RemoveSpaces()
                .ToLowerInvariant()
                .Normalize(), RouteSeparator.ToString());
        }

        private static string[] GetRouteTokens(string route)
        {
            return route.Split(RouteSeparator, StringSplitOptions.RemoveEmptyEntries);
        }

        public Route EnsureIsValid()
        {
            if (string.IsNullOrWhiteSpace(_route) || RouteTokens.Length != 0)
            {
                throw new InvalidRouteException($"Route '{_route}' is not valid");
            }

            return this;
        }

        public override string ToString() => _route;
    }
}
