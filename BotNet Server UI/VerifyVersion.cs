// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
namespace BotNet_Server_UI
{
    class VerifyVersion
    {
        public string version { get; set; }
        public bool isDeprecated { get; set; }
        public bool isUpdateNeeded { get; set; }
        public bool isNotLatest { get; set; }
        public string[] cmdlib { get; set; }
        public string[] m3md2 { get; set; }
        public string[] m3md2_startup { get; set; }
    }
}
