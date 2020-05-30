// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;

namespace m3md2
{
    public static partial class StaticVariables
    {
        public static class Diagnostics
        {
            public static string ProgramInfo { get; set; }
            public static int ExceptionCount = 0;
            public static List<Exception> exceptions = new List<Exception>();
        }
        public static class Settings
        {
            public static string ColorTheme { get; set; }
            public static bool IgnoreBigLog { get; set; }
            public static List<bool> IsDataProblem = new List<bool>();
        }
        public static class Windows
        {
            public static string InfinityListen { get; set; }
        }
    }
}
