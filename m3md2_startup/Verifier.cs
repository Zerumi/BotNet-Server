using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m3md2_startup
{
    static class Verifier
    {
        public const string version = "1.1";
        public static (string, int) Verify()
        {
            return (version, 0);
        }
    }
}
