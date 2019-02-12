using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common;

namespace ServiceFairy.Management
{
    static class Settings
    {
        public static string ServicePath
        {
            get { return CommonUtility.GetFromAppConfig<string>("servicePath", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)); }
        }

        public static string PlatformSyncPath
        {
            get { return CommonUtility.GetFromAppConfig<string>("platformSyncPath", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)); }
        }

        public static string[] PlatformSyncPathFilters
        {
            get 
            {
                string s = CommonUtility.GetFromAppConfig<string>("platformSyncPath_IgnoreFiles");
                if (string.IsNullOrWhiteSpace(s))
                    return Array<string>.Empty;

                return s.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
