using System;

namespace Common
{
    public static class Ext
    {
        public static bool Contains(this string source, string value, bool ignoreCase)
        {
            if (source == null || value == null)
                return false;
            else if (!ignoreCase)
                return source.Contains(value);
            else
                return source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string Or(this string str)
        {
            return str ?? string.Empty;
        }

        public static string Left(this string str, int len, string append = null)
        {
            if (str == null || str.Length <= len)
                return str;

            return str.Substring(0, len) + append;
        }
    };
}
