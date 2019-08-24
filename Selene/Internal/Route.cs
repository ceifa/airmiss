using Selene.Internal.Exceptions;
using Selene.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Selene.Internal
{
    public struct Route
    {
        private const char RouteSeparator = '/';
        private static readonly Regex RouteRepetitionsRegex = new Regex($"{Regex.Escape(RouteSeparator.ToString())}+", RegexOptions.Compiled);

        private string[] RouteTokens => _routeTokens.Value;

        private readonly string _route;
        private readonly Lazy<string[]> _routeTokens;

        public Route(Uri route) : this(route.AbsoluteUri)
        {
        }

        public Route(string route)
        {
            _route = route;
            _routeTokens = new Lazy<string[]>(() => GetRouteTokens(SanitizeRoute(route)));
        }

        public Route EnsureIsValid()
        {
            if (string.IsNullOrWhiteSpace(_route))
                throw new InvalidRouteException("Route cannot be empty");

            if (RouteTokens.Length == 0)
                throw new InvalidRouteException(_route);

            return this;
        }

        public Route Combine(Route target)
        {
            return new Route($"{this}{RouteSeparator}{target}");
        }

        public static Route operator +(Route left, Route right) => left.Combine(right);

        public override string ToString() => _route;

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
    }
}
