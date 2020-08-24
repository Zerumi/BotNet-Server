using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotNet_API.Models
{
    public class Info
    {
        public string uri { get; set; }
        public uint version { get; set; }
        public int port { get; set; }
        public string environment { get; set; }
        public uint clients { get; set; }
        public uint messages { get; set; }
    }
}