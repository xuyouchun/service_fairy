using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ServiceFairy.Management.Web
{
    public static class AppSettings
    {
        public static readonly string NavigationUrl = ConfigurationManager.AppSettings.Get("NavigationUrl") ?? "net.wtcp://127.0.0.1:8090";

        public static readonly string UserName = ConfigurationManager.AppSettings.Get("UserName") ?? "su";

        public static readonly string Password = ConfigurationManager.AppSettings.Get("Password") ?? "";
    }
}