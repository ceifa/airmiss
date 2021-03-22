using System.Text.RegularExpressions;

namespace Airmiss.Internal
{
    internal static class RouteSanitizer
    {
        private static readonly string RouteSeparator = Airmiss.Processor.Route.RouteSeparator.ToString();

        private static readonly Regex RouteRepetitionsRegex =
            new($"{Regex.Escape(RouteSeparator)}+", RegexOptions.Compiled);

        public static string SanitizeRoute(string plainRoute)
        {
            return RouteRepetitionsRegex.Replace(
                plainRoute
                    .Trim(Airmiss.Processor.Route.RouteSeparator)
                    .ToLowerInvariant()
                    .Normalize(), RouteSeparator);
        }
    }
}