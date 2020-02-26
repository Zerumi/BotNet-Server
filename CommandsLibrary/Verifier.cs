using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandsLibrary
{
    public static class Verifier
    {
        public static string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static (string, string) Verify()
        {
            return ("cmdlib",version);
        }
    }
}
