using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsLibrary
{
    class Verifier
    {
        public const string version = "2.1";
        public static (string, int) Verify()
        {
            return (version, 0);
        }
    }
}
