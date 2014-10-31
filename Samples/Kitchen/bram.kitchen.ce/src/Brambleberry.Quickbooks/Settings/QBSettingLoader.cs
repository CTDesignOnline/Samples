using System;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using Umbraco.Core.Logging;

namespace Brambleberry.Quickbooks.Settings
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class QBSettingLoader
    {
        /// <summary>
        /// Private constructor
        /// </summary>
        private QBSettingLoader() { }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadConfig<T>() where T : class
        {
            return LoadConfig<T>(null);
        }

        /// <summary>
        /// Return Settings object from cache or from the XML file  
        /// </summary>
        /// <typeparam name="T">The type we will passing</typeparam>
        /// <param name="fileName"> </param>
        /// <returns></returns>
        ///
        public static T LoadConfig<T>(string fileName) where T : class
        {
            T configObj = null;
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = HttpContext.Current.Server.MapPath
                    (string.Concat("~/Config/", typeof(T).Name, ".config"));
                }
                string cacheKey = fileName;
                // Load the setting object from HttpRuntime Cache if the Settings.xml
                // file is not change recently after last Chache.
                configObj = HttpRuntime.Cache[cacheKey] as T;
                if (configObj == null)// Try populate the config from cache
                {
                    configObj = LoadFromXml<T>(fileName);
                    // insert the config instance into cache use CacheDependency
                    HttpRuntime.Cache.Insert(cacheKey, configObj,
                      new System.Web.Caching.CacheDependency(fileName));
                }
            }
            catch (Exception ex)
            {
                //write error log
                LogHelper.Info(typeof(QBSettings), ex.ToString());
                return null;
            }
            return configObj;
        }
        /// <summary>
        /// Load the settings XML file and retun the Settings Type with Deserialize with XML content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">File Name of the custom XML Settings file</param>
        /// <returns>The T type which we have have paased with LoadFromXml<T> </returns>
        private static T LoadFromXml<T>(string fileName) where T : class
        {
            FileStream fs = null;
            try
            {
                //Serialize of the Type
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return (T)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                //write error log
                LogHelper.Info(typeof(QBSettings), ex.ToString());
                return null;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }
}