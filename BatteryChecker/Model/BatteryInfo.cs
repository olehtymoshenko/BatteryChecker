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

        protected BatteryInfo()
        {
            batteryInfo = new Dictionary<string, string>();
            IGNORABLE_PROPERTIES_NAME = new List<string>();
        }

        protected bool InsertPairToDictionary(string key, string value)
        {
            if (!IGNORABLE_PROPERTIES_NAME.Contains(key))
            {
                batteryInfo.Add(key, value);
                return true;
            }

            return false;
        }
    }
}
