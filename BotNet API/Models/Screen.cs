﻿// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.Collections.Generic;

namespace BotNet_API.Models
{
    public class Screen
    {
        public uint id { get; set; }
        public List<ScreenByte> screens { get; set; }
    }
}