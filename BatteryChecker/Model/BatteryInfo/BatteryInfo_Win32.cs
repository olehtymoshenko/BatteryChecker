using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

/// <summary>
/// Namespace for all battery info sources 
/// </summary>
namespace BatteryChecker.Model.BatteryInfo
{
    /// <summary>
    /// Class for getting information about battery from Win32 API 
    /// </summary>
    public class BatteryInfo_Win32:BatteryInfo
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BatteryInfo_Win32()
        {
            IGNORABLE_PROPERTIES_NAME.AddRange(new string[] { "Capabilities", "Reserved",
                "DesignedCapacity", "FullChargedCapacity", "CycleCount", "Technology"});
        }

        /// <summary>
        /// Constructor with setting up ignorable params
        /// </summary>
        /// <param name="ignorableProp">params(names) which will not be returned</param>
        public BatteryInfo_Win32(string[] ignorableProp) : this()
        {
            foreach (string prop in ignorableProp)
            {
                IGNORABLE_PROPERTIES_NAME.Add(prop);
            }
        }

        /// <summary>
        /// Method for getting information about battery
        /// </summary>
        /// <returns>Dictionary with pairs - property names, property value</returns>
        public override Dictionary<string, string> GetBatteryInfo()
        {
            BATTERY_INFORMATION bi = new BATTERY_INFORMATION();
            if (LibWrap_Win32_BatteryInfo.GetBatteryInfo(ref bi))
            {
                ConvertBATT_INFO_To_Dictionary(bi);
                return base.batteryInfo;
            }
            return null;
        }

        /// <summary>
        /// Method for converting struct BATTERY_INFO to dictionary<string, string>
        /// </summary>
        /// <param name="bi">Struct with battery information for coverting to dictionary</param>
        private void ConvertBATT_INFO_To_Dictionary(BATTERY_INFORMATION bi)
        {
            Type biType = bi.GetType();
            FieldInfo[] fiArr = biType.GetFields();

            foreach(FieldInfo fi in fiArr)
            {
                if (fi.FieldType.IsArray)
                {
                    base.InsertPairToDictionary(fi.Name, (Encoding.UTF8.GetString((byte[])fi.GetValue(bi))).TrimEnd('\0'));
                }
                else
                {
                    base.InsertPairToDictionary(fi.Name, fi.FieldType==typeof(byte)?((byte)fi.GetValue(bi)).ToString():fi.GetValue(bi).ToString());
                }
            }
        }
    }

    /// <summary>
    /// Class-wrapper for native functions for getting battery information from win32 api
    /// </summary>
    public static class LibWrap_Win32_BatteryInfo
    {
        [DllImport("BatteryInfo_Win32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetBatteryInfo(ref BATTERY_INFORMATION bi);
    }

    /// <summary>
    /// Struct for method GetBatteryInfo, represent battery info from win32 api
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BATTERY_INFORMATION
    {
        public uint Capabilities;
        public byte Technology;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =3 )]
        public byte[] Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Chemistry;
        public uint DesignedCapacity;
        public uint FullChargedCapacity;
        public uint DefaultAlert1;
        public uint DefaultAlert2;
        public uint CriticalBias;
        public uint CycleCount;
    }
}
