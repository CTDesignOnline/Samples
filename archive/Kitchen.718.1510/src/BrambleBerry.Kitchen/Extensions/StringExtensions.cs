using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrambleBerry.Kitchen.Extensions
{
    public static class StringExtensions
    {
        public static string EnsureNonSurfaceUrl(this string url)
        {
            if (url.StartsWith("/umbraco/Surface/"))
            {
                url = url.Replace("/umbraco/Surface", "");
            }
            return url;
        }
    }
}
