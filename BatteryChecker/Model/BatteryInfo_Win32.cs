using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;

namespace BatteryChecker.Model
{
    public class BatteryInfo_Win32:BatteryInfo
    {
        public BatteryInfo_Win32()
        {
            IGNORABLE_PROPERTIES_NAME.AddRange(new string[] { "Capabilities", "Reserved",
                "DesignedCapacity", "FullChargedCapacity", "CycleCount", "Technology"});
        }

        public BatteryInfo_Win32(string[] ignorableProp) : this()
        {
            foreach (string prop in ignorableProp)
            {
                IGNORABLE_PROPERTIES_NAME.Add(prop);
            }
        }

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


    public static class LibWrap_Win32_BatteryInfo
    {
        [DllImport("BatteryInfo_Win32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetBatteryInfo(ref BATTERY_INFORMATION bi);
    }

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
