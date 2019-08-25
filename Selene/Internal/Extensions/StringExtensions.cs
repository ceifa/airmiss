namespace Selene.Internal.Extensions
{
    internal static class StringExtensions
    {
        internal static string RemoveSpaces(this string target)
        {
            return target.Replace(" ", string.Empty);
        }
    }
}