using System;
using System.Collections.Generic;
using BatteryChecker.Model.Translators;

/// <summary>
/// Namespace for all battery info sources 
/// </summary>
namespace BatteryChecker.Model.BatteryInfo
{
    /// <summary>
    /// Abstract class for all battery information source
    /// </summary>
    public abstract class BatteryInfo
    {
        /// <summary>
        /// Contain info about battery 
        /// </summary>
        protected Dictionary<string, string> batteryInfo;

        /// <summary>
        /// Method for getting information about battery
        /// </summary>
        /// <returns>Dictionary with pairs - property names, property value</returns>
        public abstract Dictionary<string, string> GetBatteryInfo();

        /// <summary>
        /// field to contain names of properties which need to ignore (dont insert in result dictionaty)
        /// </summary>
        protected readonly List<string> IGNORABLE_PROPERTIES_NAME;

        /// <summary>
        /// Translator from eng to rus (translate name and values of battery properties)
        /// </summary>
        private EnToRusTranslator Translator { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected BatteryInfo()
        {
            batteryInfo = new Dictionary<string, string>();
            IGNORABLE_PROPERTIES_NAME = new List<string>();
            Translator = EnToRusTranslator.GetInstance();
        }

        /// <summary>
        /// Insert new pair(name, value) to dictionary with information about battery, method try unbox value from object
        /// </summary>
        /// <param name="key">param name (key for dictionary)</param>
        /// <param name="value">param value (value in dictionary)</param>
        /// <returns></returns>
        protected bool InsertPairToDictionary(string key, object value)
        {
            if (!IGNORABLE_PROPERTIES_NAME.Contains(key))
            {
                string unboxedValue = UnboxValue(value);
                string translatedKey = Translator.Translate(key);
                string translatedValue = Translator.Translate(unboxedValue);

                translatedKey = translatedKey ?? key;
                translatedValue = translatedValue ?? unboxedValue;

                batteryInfo.Add(translatedKey, translatedValue);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Insert new pair(name, value) to dictionary with information about battery
        /// </summary>
        /// <param name="key">param name (key for dictionary)</param>
        /// <param name="value">param value (value in dictionary)</param>
        /// <returns></returns>
        protected bool InsertPairToDictionary(string key, string value)
        {
            if (!IGNORABLE_PROPERTIES_NAME.Contains(key))
            {
                string translatedKey = Translator.Translate(key);
                string translatedValue = Translator.Translate(value);

                translatedKey = translatedKey ?? key;
                translatedValue = translatedValue ?? value;

                batteryInfo.Add(translatedKey, translatedValue);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method for unboxing object value to string, indeed method can detect only Timespan and convert
        /// to string with formating, other values will convert by method ToString()
        /// </summary>
        /// <param name="value">Object for unboxing</param>
        /// <returns>return string, which represent object value</returns>
        private string UnboxValue(object value)
        {
            if (value.GetType() == typeof(TimeSpan))
            {
                TimeSpan.TryParse(value.ToString(), out TimeSpan ts);
                 return ts.ToString(@"hh\:mm\:ss");
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
