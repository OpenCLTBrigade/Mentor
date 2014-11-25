using System;
using System.Collections.Generic;
using System.Collections.Specialized;

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

        public static void Merge(this IDictionary<string, object> dict, NameValueCollection form)
        {
            foreach (string key in form)
            {
                dict[key] = form[key];
            }
        }

        //unit = 'M'=miles, 'K'=kilometers, 'N'=nautical miles
        public static double? CalcDist(double? lat1, double? lon1, double? lat2, double? lon2, string unit = "M")
        {
            if (!lat1.HasValue || !lon1.HasValue || !lat2.HasValue || !lon2.HasValue) return null;
            var radlat1 = Math.PI*lat1.Value/180d;
            var radlat2 = Math.PI*lat2.Value/180d;
            var theta = lon1.Value - lon2.Value;
            var radtheta = Math.PI*theta/180d;
            var dist = Math.Sin(radlat1)*Math.Sin(radlat2) + Math.Cos(radlat1)*Math.Cos(radlat2)*Math.Cos(radtheta);
            dist = Math.Acos(dist);
            dist = dist*180d/Math.PI;
            dist = dist*60d*1.1515d;
            if (unit == "K") dist = dist*1.609344d;
            if (unit == "N") dist = dist*0.8684d;
            return dist;
        }
    };
}
