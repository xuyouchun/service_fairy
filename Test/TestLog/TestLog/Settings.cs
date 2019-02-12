using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace TestLog
{
    static class Settings
    {
        public static string LogPath
        {
            get
            {
                return CommonUtility.GetFromAppConfig<string>("logpath", @"c:\log\logtest");
            }
        }

        public static int Count
        {
            get { return CommonUtility.GetFromAppConfig<int>("count", 10000); }
        }

        public static int ThreadCount
        {
            get { return CommonUtility.GetFromAppConfig<int>("threadCount", 10); }
        }

        public static string Navigation
        {
            get { return CommonUtility.GetFromAppConfig<string>("navigation", "117.79.130.229:80"); }
        }
    }
}
