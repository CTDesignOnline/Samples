namespace Brambleberry.Quickbooks.Settings
{
    /// <summary>
    /// This manager helper class will provide the  Settings
    /// </summary>
    public class SettingsManager
    {
        /// <summary>
        /// This static property return the Settings class object from loading the configuration
        /// </summary>
        public static QBSettings QBSettings
        {
            get
            {
                return QBSettingLoader.LoadConfig<QBSettings>();
            }
        }
    }
}