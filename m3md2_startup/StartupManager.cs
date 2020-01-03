// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Configuration;
using m3md2;

namespace m3md2_startup
{
    public class StartupManager
    {
        public static void Main()
        {
            var appSettings = ConfigurationManager.AppSettings;
            StaticVariables.Settings.ColorTheme = appSettings.Get("ColorTheme");
            StaticVariables.Windows.InfinityListen = appSettings.Get("InfnityListen");
            StaticVariables.Settings.IgnoreBigLog = Convert.ToBoolean(appSettings.Get("IgnoreBigLog"));
        }
    }
}
