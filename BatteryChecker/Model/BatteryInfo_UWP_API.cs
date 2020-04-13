using System;
using System.Collections.Generic;
using Windows.Devices.Power; // uwp api
using Windows.System.Power;  // uwp 
using System.Reflection;

namespace BatteryChecker.Model
{
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

        public BatteryInfo_UWP_API()
        {
            DataSourceMode = DataSource.AllInformation;
            IGNORABLE_PROPERTIES_NAME.Add("BatteryStatus");
        }

        public BatteryInfo_UWP_API(DataSource mode):this()
        {
            DataSourceMode = mode;
        }

        public BatteryInfo_UWP_API(string[] newIgnorobleProperties) : this()
        {
            foreach(string propName in newIgnorobleProperties)
            {
                IGNORABLE_PROPERTIES_NAME.Add(propName);
            }
        }

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
