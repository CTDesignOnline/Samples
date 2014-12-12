namespace BrambleBerry.Kitchen
{
    using System;
    using Merchello.Core.Models;

    /// <summary>
    /// The extended data extensions.
    /// </summary>
    /// <remarks>
    /// TODO move these to Merchello.Core
    /// </remarks>
    public static class ExtendedDataExtensions
    {
        /// <summary>
        /// The get value as int.
        /// </summary>
        /// <param name="extendedData">
        /// The extended data.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetValueAsInt(this ExtendedDataCollection extendedData, string key)
        {
            int converted;
            return int.TryParse(extendedData.GetValue(key), out converted) ? converted : 0;
        }

        /// <summary>
        /// The get value as guid.
        /// </summary>
        /// <param name="extendedData">
        /// The extended data.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid GetValueAsGuid(this ExtendedDataCollection extendedData, string key)
        {
            Guid converted;
            return Guid.TryParse(extendedData.GetValue(key), out converted) ? converted : Guid.Empty;
        }

        /// <summary>
        /// The get value as bool.
        /// </summary>
        /// <param name="extendedData">
        /// The extended data.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool GetValueAsBool(this ExtendedDataCollection extendedData, string key)
        {
            bool converted;
            return bool.TryParse(extendedData.GetValue(key), out converted) && converted;
        }

        /// <summary>
        /// The get value as decimal.
        /// </summary>
        /// <param name="extendedData">
        /// The extended data.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="decimal"/>.
        /// </returns>
        public static decimal GetValueAsDecimal(this ExtendedDataCollection extendedData, string key)
        {
            decimal converted;
            return decimal.TryParse(extendedData.GetValue(key), out converted) ? converted : 0;
        }
    }
}