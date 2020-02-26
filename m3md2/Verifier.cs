using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace m3md2
{
    public static class Verifier
    {
        public static string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static (string, string) Verify()
        {
            return ("m3md2",version);
        }
    }
}
