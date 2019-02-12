using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace ServiceFairy.Client.WinForm
{
    static class Settings
    {
        private static UserInfo[] _userInfos;

        public static UserInfo[] GetUserInfos()
        {
            if (_userInfos == null)
            {
                string users = CommonUtility.GetFromAppConfig<string>("users");
                if (!string.IsNullOrEmpty(users))
                {
                    var list = from s in users.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                               where !string.IsNullOrWhiteSpace(s)
                               let k = s.IndexOf(':')
                               let username = (k < 0) ? s : s.Substring(0, k)
                               let password = (k < 0) ? "" : s.Substring(k + 1)
                               select new UserInfo(username.Trim(), password);

                    _userInfos = list.ToArray();
                }
            }

            return _userInfos;
        }

        public static string NavigationUrl
        {
            get { return CommonUtility.GetFromAppConfig<string>("navigationUrl", "net.wtcp://127.0.0.1:8090"); }
        }
    }
}
