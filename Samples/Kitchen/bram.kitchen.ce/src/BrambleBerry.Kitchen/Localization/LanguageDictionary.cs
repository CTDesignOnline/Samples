using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrambleBerry.Kitchen.Localization
{
    public static class LanguageDictionary
    {
        /// <summary>Various site wide error message text, kept in one place for ease and for possible translations</summary>
        public static class Alerts
        {
            // TODO - wire these up to an Umbraco dictionary at some point if you want multi lingual goodness
            public static string InvalidUsernameOrPassword = "Invalid username or password"; 
        }
    }
}
