// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace m3md2_startup
{
    public static class Verifier
    {
        public static string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static (string, string) Verify()
        {
            return ("m3md2_startup",version);
        }
    }
}
