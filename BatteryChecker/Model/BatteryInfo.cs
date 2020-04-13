using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryChecker.Model
{
    public abstract class BatteryInfo
    {
        /// <summary>
        /// Contain info about battery 
        /// </summary>
        protected Dictionary<string, string> batteryInfo;

        /// <summary>
        /// Method for getting information about battery
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, string> GetBatteryInfo();

        /// <summary>
        /// field to contain names of properties which need to ignore (dont insert in result dictionaty)
        /// </summary>
        protected readonly List<string> IGNORABLE_PROPERTIES_NAME;

        private EnToRusTranslator Translator { get; set; }

        protected BatteryInfo()
        {
            batteryInfo = new Dictionary<string, string>();
            IGNORABLE_PROPERTIES_NAME = new List<string>();
            Translator = EnToRusTranslator.GetInstance();
        }

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
