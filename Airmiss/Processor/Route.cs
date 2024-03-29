﻿using System;
using System.Collections.Generic;
using System.Linq;
using Airmiss.Exceptions;
using Airmiss.Internal;

namespace Airmiss.Processor
{
    public readonly struct Route
    {
        internal const char RouteSeparator = '/';

        private readonly string _route;
        private readonly RouteToken[] _routeTokens;

        public Route(Uri route) : this(route.AbsolutePath)
        {
        }

        public Route(string route)
        {
            _route = RouteSanitizer.SanitizeRoute(route);
            _routeTokens = GetRouteTokens(_route);

            EnsureIsValid();
        }

        public static bool Match(Route source, Route target)
        {
            if (source._routeTokens.Length != target._routeTokens.Length)
                return false;

            return !source._routeTokens
                .Where((t, i) => t.ToString() != target._routeTokens[i].ToString() && !t.IsVariable)
                .Any();
        }

        public Route EnsureIsValid()
        {
            if (string.IsNullOrWhiteSpace(_route))
                throw new InvalidRouteException(_route, "Cannot be empty");

            if (_route.Contains(' '))
                throw new InvalidRouteException(_route, "Cannot have whitespaces");

            if (_routeTokens?.Length == 0)
                throw new InvalidRouteException(_route);

            return this;
        }

        public Route Combine(Route target)
        {
            return new($"{this}{RouteSeparator}{target}");
        }

        public Dictionary<string, string> GetVariableValues(Route populatedRoute)
        {
            if (!Match(this, populatedRoute))
                throw new InvalidOperationException("Tried to get variables from unmatched routes");

            return _routeTokens.Zip(populatedRoute._routeTokens, (t1, t2) => (t1, t2))
                .Where(routes => routes.t1.IsVariable)
                .ToDictionary(routes => routes.t1.VariableName!, routes => routes.t2.ToString());
        }

        public override bool Equals(object? obj)
        {
            if (obj is Route route) return _route.Equals(route._route);

            return false;
        }

        public override string ToString()
        {
            return _route;
        }

        public override int GetHashCode()
        {
            return _route.GetHashCode();
        }

        private static RouteToken[] GetRouteTokens(string route)
        {
            return route.Split(RouteSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => new RouteToken(t)).ToArray();
        }

        public static bool operator ==(Route left, Route right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Route left, Route right)
        {
            return !left.Equals(right);
        }

        public static Route operator +(Route route1, Route route2)
        {
            return route1.Combine(route2);
        }

        public static implicit operator Route(string route)
        {
            return new(route);
        }

        public static implicit operator Route(Uri route)
        {
            return new(route);
        }
    }
}