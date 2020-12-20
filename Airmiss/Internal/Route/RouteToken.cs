using System;
using System.Text.RegularExpressions;

namespace Airmiss.Internal
{
    internal class RouteToken
    {
        private const string VariableNameRegexGroup = nameof(VariableName);

        private static readonly Regex VariableRegex =
            new Regex($"^{{(?<{VariableNameRegexGroup}>.*?)}}$", RegexOptions.Compiled);

        private readonly string _token;
        private readonly Lazy<string?> _variableNameLazy;

        public RouteToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Route token cannot be null or white space", nameof(token));

            _token = token;
            _variableNameLazy = new Lazy<string?>(GetVariableName);
        }

        public bool IsVariable => VariableName != null;

        public string? VariableName => _variableNameLazy.Value;

        public override string ToString()
        {
            return _token;
        }

        private string? GetVariableName()
        {
            var match = VariableRegex.Match(_token);
            if (!match.Success)
                return null;

            var variableNameMatchGroup = match.Groups[VariableNameRegexGroup];
            if (!variableNameMatchGroup.Success || string.IsNullOrWhiteSpace(variableNameMatchGroup.Value))
                return null;

            return variableNameMatchGroup.Value;
        }
    }
}