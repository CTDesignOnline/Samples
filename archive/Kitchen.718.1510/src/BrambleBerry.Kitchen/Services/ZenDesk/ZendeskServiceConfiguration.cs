using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchello.Core.Configuration;

namespace BrambleBerry.Kitchen.Services.ZenDesk
{
    public static class ZendeskServiceConfiguration
    {

        /// <summary>
        /// Returns whether or not to use the live provider for the ZendeskLogic
        /// </summary>
        public static Boolean UseLiveProvider
        {
            get { return Get<bool>("Zendesk:UseLive"); }
        }

        /// <summary>
        /// Endpoint to use for live
        /// </summary>
        public static string EndpointUrl
        {
            get { return Get<string>("Zendesk:EndpointUrl"); }
        }

        /// <summary>
        /// Returns the setting value for the development environment.
        /// </summary>
        public static string Username
        {
            get { return Get<string>("Zendesk:Username"); }
        }

        /// <summary>
        /// Returns the setting description.
        /// </summary>
        public static string Password
        {
            get { return Get<string>("Zendesk:Password"); }
        }

        private static T Get<T>(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(appSetting)) throw new Exception("app setting not found:" + key);

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }

    }
}
