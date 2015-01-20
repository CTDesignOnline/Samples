using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrambleBerry.Kitchen.FormLogic
{
    internal static class Extensions
    {
  

        /// <summary>
        /// Turns object into dictionary
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ignoreProperties">Properties to ignore</param>
        /// <returns></returns>
        internal static IDictionary<string, TVal> ToDictionary<TVal>(this object o, params string[] ignoreProperties)
        {
            if (o != null)
            {
                var props = TypeDescriptor.GetProperties(o);
                var d = new Dictionary<string, TVal>();
                foreach (var prop in props.Cast<PropertyDescriptor>().Where(x => !ignoreProperties.Contains(x.Name)))
                {
                    var val = prop.GetValue(o);
                    if (val != null)
                    {
                        d.Add(prop.Name, (TVal)val);
                    }
                }
                return d;
            }
            return new Dictionary<string, TVal>();
        }

        /// <summary>
        /// Converts a dictionary object to a query string representation such as:
        /// firstname=shannon&lastname=deminick
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToQueryString(this IDictionary<string, object> d)
        {
            if (!d.Any()) return "";

            var builder = new StringBuilder();
            foreach (var i in d)
            {
                builder.Append(String.Format("{0}={1}&", HttpUtility.UrlEncode(i.Key), i.Value == null ? string.Empty : HttpUtility.UrlEncode(i.Value.ToString())));
            }
            return builder.ToString().TrimEnd('&');
        }
    }
}
