using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Selene.Exceptions;
using Selene.Internal.Extensions;

namespace Selene.Messaging
{
    public struct Route
    {
        private const char RouteSeparator = '/';

        private static readonly Regex RouteRepetitionsRegex =
            new Regex($"{Regex.Escape(RouteSeparator.ToString())}+", RegexOptions.Compiled);

        private RouteToken[] RouteTokens => _routeTokensLazy.Value;

        private readonly string _route;
        private readonly Lazy<RouteToken[]> _routeTokensLazy;

        public Route(Uri route) : this(route.AbsoluteUri)
        {
        }

        public Route(string route)
        {
            _route = route;
            _routeTokensLazy = new Lazy<RouteToken[]>(() => GetRouteTokens(SanitizeRoute(route)));
        }

        public Route EnsureIsValid()
        {
            if (string.IsNullOrWhiteSpace(_route))
                throw new InvalidRouteException("Route cannot be empty");

            if (RouteTokens?.Length == 0)
                throw new InvalidRouteException(_route);

            return this;
        }

        public Route Combine(Route target)
        {
            return new Route($"{this}{RouteSeparator}{target}");
        }

        public Dictionary<string, string> GetVariables(Route target)
        {
            if (Match(this, target))
                throw new InvalidOperationException("Tried to get variables from unmatched routes");

            return RouteTokens.Zip(target.RouteTokens,
                    (r1, r2) => new KeyValuePair<string, string>(r1.VariableName, r2.ToString()))
                .ToDictionary(t => t.Key, t => t.Value);
        }

        public static bool Match(Route route1, Route route2)
        {
            if (route1.EnsureIsValid().RouteTokens.Length != route2.EnsureIsValid().RouteTokens.Length)
                return false;

            return !route1.RouteTokens.Where((t, i) => t.VariableName == null && t != route2.RouteTokens[i]).Any();
        }

        public static Route operator +(Route route1, Route route2)
        {
            return route1.Combine(route2);
        }

        public override string ToString()
        {
            return _route;
        }

        private static string SanitizeRoute(string route)
        {
            return RouteRepetitionsRegex.Replace(route
                .Trim(RouteSeparator)
                .RemoveSpaces()
                .ToLowerInvariant()
                .Normalize(), RouteSeparator.ToString());
        }

        private static RouteToken[] GetRouteTokens(string route)
        {
            return route.Split(RouteSeparator, StringSplitOptions.RemoveEmptyEntries).Select(t => new RouteToken(t))
                .ToArray();
        }
    }
}