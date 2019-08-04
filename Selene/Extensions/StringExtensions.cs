using System.IO;

namespace Selene.Extensions
{
    public static class StringExtensions
    {
        public static string SanitizePath(this string path)
        {
            return Path.TrimEndingDirectorySeparator(path.Trim());
        }
    }
}