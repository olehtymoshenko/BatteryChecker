using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

/// <summary>
/// Namespace for all battery info sources 
/// </summary>
namespace BatteryChecker.Model.BatteryInfo
{
    /// <summary>
    /// Class for getting information about battery from WMI
    /// </summary>
    public class BatteryInfo_WMI : BatteryInfo
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BatteryInfo_WMI()
        {
            IGNORABLE_PROPERTIES_NAME.AddRange( new string[] { "EstimatedChargeRemaining", "Status",
                "Availability", "BatteryStatus", "Chemistry", "Description", "CreationClassName",
                "EstimatedRunTime", "PowerManagementCapabilities", "EstimatedRunTime",
                "SystemCreationClassName", "PowerManagementSupported"});
        }

        /// <summary>
        /// Constructor with setting up ignorable params
        /// </summary>
        /// <param name="ignorableProp">params(names) which will not be returned</param>
        public BatteryInfo_WMI(string[] ignorableProp) : this()
        {
            foreach (string prop in ignorableProp)
            {
                IGNORABLE_PROPERTIES_NAME.Add(prop);
            }
        }

        /// <summary>
        /// Method for getting information about battery from WMI class - Win32_Battery
        /// </summary>
        /// <returns>Dictionary with pairs - property names, property value</returns>
        public override Dictionary<string, string> GetBatteryInfo()
        {
            try
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
            catch(Exception e)
            {
                throw new Exception("Не удалось получить информацию от WMI\n" + e.Message);
            }
        }
    }
}
