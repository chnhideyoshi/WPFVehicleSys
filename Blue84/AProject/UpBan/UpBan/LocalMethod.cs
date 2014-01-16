using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace UpBan
{
    public static class LocalMethod
    {

        internal static bool GetUSBExistence()
        {
            return Environment.GetLogicalDrives().Length > 1;
        }

        internal static bool GetBlueToothExistence()
        {
            return true;
        }

        internal static bool Get3GExistence()
        {
            return true;
        }

        internal static bool GetMuteExistence()
        {
            return false;
        }
       
        internal static double GetBatteryPercentage()
        {
            return 1;
        }

        #region PInvoke
        [DllImport("Winmm.dll")]
        private static extern int waveOutSetVolume(int hwo, System.UInt32 pdwVolume);//
        [DllImport("Winmm.dll")]
        private static extern uint waveOutGetVolume(int hwo, out System.UInt32 pdwVolume); 
        #endregion
    }
}
