using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Client;
using Common.Framework.TrayPlatform;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Master.Components
{
    static class MasterUtility
    {
        public static int GetOnlineUserCount(this AppClientInfo clientInfo)
        {
            OnlineUserStatInfo statInfo;
            if (clientInfo == null || clientInfo.RuntimeInfo == null || (statInfo = clientInfo.RuntimeInfo.OnlineUserStatInfo) == null)
                return 0;

            return statInfo.CurrentOnlineUserCount;
        }
    }
}
