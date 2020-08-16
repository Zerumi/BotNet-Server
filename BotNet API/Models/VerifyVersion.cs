using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotNet_API.Models
{
    public class VerifyVersion
    {
        public string version { get; set; }
        public bool isDeprecated { get; set; }
        public bool isUpdateNeeded { get; set; }
        public bool isNotLatest { get; set; }
        public string[] cmdlib { get; set; }
        public string[] m3md2 { get; set; }
        public string[] m3md2_startup { get; set; }
        public string custommessage { get; set; }
    }
}
