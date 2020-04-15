using System;
using System.Collections.Generic;
using Windows.Devices.Power; // for UWP API
using Windows.System.Power;  // for UWP API
using System.Reflection;

/// <summary>
/// Namespace for all battery info sources 
/// </summary>
namespace BatteryChecker.Model.BatteryInfo
{
    /// <summary>
    /// Class for getting information about battery from UWP API
    /// </summary>
    public class BatteryInfo_UWP_API : BatteryInfo
    {
        /// <summary>
        /// Enum using for detect which information need to output
        /// </summary>
        public enum DataSource
        {
            AllInformation = 0,
            OnlyBatteryReportClass,
            OnlyPowerManagerClass
        };

        private DataSource dataSourceMode;

        /// <summary>
        /// Property set which information will be returned
        /// </summary>
        public DataSource DataSourceMode
        {
            get
            {
                return dataSourceMode;
            }
            set
            {
                if(value >= DataSource.AllInformation && value <= DataSource.OnlyPowerManagerClass)
                {
                    if (Enum.IsDefined(typeof(DataSource), value))
                    {
                        dataSourceMode = value;
                    }
                }
            }
        }

        /// <summary>
        /// Default constuctor
        /// </summary>
        public BatteryInfo_UWP_API()
        {
            DataSourceMode = DataSource.AllInformation;
            IGNORABLE_PROPERTIES_NAME.Add("BatteryStatus");
        }

        /// <summary>
        /// Constructor with setting up returning information mode
        /// </summary>
        /// <param name="mode">Set up which information will be returned</param>
        public BatteryInfo_UWP_API(DataSource mode):this()
        {
            DataSourceMode = mode;
        }

        /// <summary>
        /// Constructor with setting up returning information mode
        /// </summary>
        /// <param name="newIgnorobleProperties">params(names) which will not be returned</param>
        public BatteryInfo_UWP_API(string[] newIgnorobleProperties) : this()
        {
            foreach(string propName in newIgnorobleProperties)
            {
                IGNORABLE_PROPERTIES_NAME.Add(propName);
            }
        }

        /// <summary>
        /// Method for getting information about battery
        /// </summary>
        /// <returns>Dictionary with pairs - property names, property value</returns>
        public override Dictionary<string, string> GetBatteryInfo()
        {
            switch (DataSourceMode)
            {
                case DataSource.AllInformation:
                    {
                        GetInfoFromBatteryReport(); // get info from BatteryReport class
                        GetInfoFromPowerManager(); // get info from PowerManager class
                    }
                    break;
                case DataSource.OnlyBatteryReportClass:
                    {
                        GetInfoFromBatteryReport(); // get info from BatteryReport class
                    }
                    break;
                case DataSource.OnlyPowerManagerClass:
                    {
                        GetInfoFromPowerManager(); // get info from PowerManager class
                    }
                    break;
            }
            return batteryInfo;
        }

        /// <summary>
        /// Get battery info from BatteryReport class
        /// </summary>
        private void GetInfoFromBatteryReport()
        {
            BatteryReport battRep = Battery.AggregateBattery.GetReport(); // get summary info of all batterys

            Type battRepType = battRep.GetType();
            PropertyInfo[] allPI = battRepType.GetProperties();

            foreach(PropertyInfo pi in allPI)
            {
                 InsertPairToDictionary(pi.Name, pi.GetValue(battRep));
            }
        }

        /// <summary>
        /// Get battery info from PowerManager class
        /// </summary>
        private void GetInfoFromPowerManager()
        {
            Type powManType = typeof(PowerManager);
            PropertyInfo[] allPI = powManType.GetProperties();

            foreach(PropertyInfo pi in allPI)
            {
                InsertPairToDictionary(pi.Name, pi.GetValue(null));
            }
        }
    }
}
