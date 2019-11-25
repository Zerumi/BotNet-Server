using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotNet_Server_UI
{
    class Info
    {
        public string uri { get; set; }
        public uint version { get; set; }
        public int port { get; set; }
        public string environment { get; set; }
        public uint clients { get; set; }
        public uint messages { get; set; }
    }
}
