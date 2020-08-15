using System.Collections.Generic;

namespace BotNet_API.Models
{
    public class Command
    {
        public uint id { get; set; }
        public string command { get; set; }
        public List<Client> Clients { get; set; }
    }
}