using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Service.Master
{
    static class Settings
    {
        public static int StartPort
        {
            get { return CommonUtility.GetFromAppConfig<int>("startPort", 9001); }
        }

        public static int EndPort
        {
            get { return CommonUtility.GetFromAppConfig<int>("endPort", 9999); }
        }
    }
}
