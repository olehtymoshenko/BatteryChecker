using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BatteryChecker.Model
{
    public class BatteryInfo_Win32:BatteryInfo
    {
        public override Dictionary<string, string> GetBatteryInfo()
        {
            BATTERY_INFORMATION bi = new BATTERY_INFORMATION();
            if (LibWrap_Win32_BatteryInfo.GetBatteryInfo(ref bi))
            {
                base.batteryInfo.Add("Chemistry", bi.Chemistry.ToString());
                return base.batteryInfo;
            }
            return null;
        }

        private void ConvertBATT_INFO_To_Dictionary(BATTERY_INFORMATION bi)
        {

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
        public char Technology;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =3 )]
        public char[] Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] Chemistry;
        public uint DesignedCapacity;
        public uint FullChargedCapacity;
        public uint DefaultAlert1;
        public uint DefaultAlert2;
        public uint CriticalBias;
        public uint CycleCount;
    }

}
