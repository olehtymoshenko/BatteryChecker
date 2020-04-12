using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace BatteryChecker.Model
{
    class BatteryInfo_WMI : BatteryInfo
    {
        public BatteryInfo_WMI()
        {
            IGNORABLE_PROPERTIES_NAME.AddRange( new string[] { "EstimatedChargeRemaining", "Status",
                "Availability", "BatteryStatus", "Chemistry", "Description", "CreationClassName",
                "EstimatedRunTime", "PowerManagementCapabilities", "EstimatedRunTime", "SystemCreationClassName"});
        }

        public BatteryInfo_WMI(string[] ignorableProp) : this()
        {
            foreach (string prop in ignorableProp)
            {
                IGNORABLE_PROPERTIES_NAME.Add(prop);
            }
        }

        public override Dictionary<string, string> GetBatteryInfo()
        {   
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\cimv2", @"SELECT * FROM Win32_Battery"))
            {
                ManagementObjectCollection collection = searcher.Get();

                StringBuilder tmpProp = new StringBuilder("");

                foreach (ManagementObject mo in collection)
                {
                    foreach (PropertyData property in mo.Properties)
                    {
                        if (property.Value != null)
                        {
                            if (property.Value.GetType().IsArray)
                            {
                                foreach (var i in (Array)property.Value)
                                {
                                    tmpProp.Append(i.ToString() + ";");
                                }
                            }
                            else
                            {
                                tmpProp.Append(property.Value);
                            }
                        }
                        else
                        {
                            continue;
                        }
                        InsertPairToDictionary(property.Name, tmpProp.ToString());
                        tmpProp.Clear();
                    }
                }
            }
            return base.batteryInfo;
        }
    }
}
