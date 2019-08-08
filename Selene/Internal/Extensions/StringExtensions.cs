using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Selene.Internal.Extensions
{
    internal static class StringExtensions
    {
        internal static string RemoveSpaces(this string target) => target.Replace(" ", string.Empty);
    }
}
