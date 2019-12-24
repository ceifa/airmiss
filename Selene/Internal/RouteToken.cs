using System;
using System.Text.RegularExpressions;

namespace Selene.Internal
{
    internal class RouteToken
    {
        private const string VariableNameRegexGroup = nameof(VariableName);

        private static readonly Regex VariableRegex =
            new Regex($"^{{(<{VariableNameRegexGroup}>.*?)}}$", RegexOptions.Compiled);

        private static readonly Regex EscapedVariableRegex = new Regex("^{{.*?}}$", RegexOptions.Compiled);

        private readonly string _token;
        private readonly Lazy<string> _variableNameLazy;

        public RouteToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Route token cannot be null or white space", nameof(token));

            _token = token;
            _variableNameLazy = new Lazy<string>(() => TryGetVariableName(out var variableName) ? variableName : null);
        }

        public bool IsVariable => VariableName != null;

        public string VariableName => _variableNameLazy.Value;

        public override string ToString()
        {
            return _token;
        }

        private bool TryGetVariableName(out string variableName)
        {
            variableName = default;

            if (EscapedVariableRegex.IsMatch(_token))
                return false;

            var match = VariableRegex.Match(_token);
            if (!match.Success)
                return false;

            var variableNameMatchGroup = match.Groups[VariableNameRegexGroup];
            if (!variableNameMatchGroup.Success || string.IsNullOrWhiteSpace(variableNameMatchGroup.Value))
                return false;

            variableName = variableNameMatchGroup.Value;
            return true;
        }
    }
}