using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;

namespace m3md2_startup
{
    public class StartupManager
    {
        public static void Main()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var colortheme = appSettings.Get("ColorTheme");
        }
    }
}
