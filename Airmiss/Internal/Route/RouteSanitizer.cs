using System.Text.RegularExpressions;
using Airmiss.Processor;

namespace Airmiss.Internal.Routing
{
    internal static class RouteSanitizer
    {
        private static readonly string RouteSeparator = Route.RouteSeparator.ToString();

        private static readonly Regex RouteRepetitionsRegex =
            new Regex($"{Regex.Escape(RouteSeparator)}+", RegexOptions.Compiled);

        public static string SanitizeRoute(string plainRoute)
        {
            return RouteRepetitionsRegex.Replace(
                plainRoute
                    .Trim(Route.RouteSeparator)
                    .ToLowerInvariant()
                    .Normalize(), RouteSeparator);
        }
    }
}