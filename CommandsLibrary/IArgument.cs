using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsLibrary
{
    public interface IArgument
    {
        public string Command { get; set; }
        public int ArgumentCount { get; set; }
        public string[] ArgumentsList { get; set; }
    }
}
