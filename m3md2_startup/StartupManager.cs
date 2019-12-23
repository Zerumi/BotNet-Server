// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.Configuration;

namespace m3md2_startup
{
    public class StartupManager
    {
        public static void Main(ref string colortheme)
        {
            var appSettings = ConfigurationManager.AppSettings;
            colortheme = appSettings.Get("ColorTheme");
        }
    }
}
