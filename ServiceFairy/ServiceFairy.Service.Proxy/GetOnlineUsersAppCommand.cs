using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using ServiceFairy.Entities.Proxy;
using Common.Utility;

namespace ServiceFairy.Service.Proxy
{
    /// <summary>
    /// 获取在线用户
    /// </summary>
    [AppCommand("GetOnlineUsers", "获取在线用户", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class GetOnlineUsersAppCommand : ACS<Service>.Func<Proxy_GetOnlineUsers_Request, Proxy_GetOnlineUsers_Reply>
    {
        protected override Proxy_GetOnlineUsers_Reply OnExecute(AppCommandExecuteContext<Service> context, Proxy_GetOnlineUsers_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            var onlineUserMgr = context.Service.OnlineUserManager;
            UserConnectionInfo[] infos = onlineUserMgr.GetAllOnlineUsers();
            _Sort(infos, request.SortType, request.SortField);

            infos = infos.Range(request.Start, request.Count);
            IDictionary<int, string> dict = context.Service.UserParser.ConvertUserIdToNamesDict(infos.ToArray(info => info.UserId));

            return new Proxy_GetOnlineUsers_Reply() {
                Infos = infos.ToArray(info => new ProxyOnlineUserInfo {
                    UserId = info.UserId, UserName = dict.GetOrDefault(info.UserId), ConnectionTime = info.ConnectionTime
                })
            };
        }

        private void _Sort(UserConnectionInfo[] infos, SortType sortType, GetOnlineUsersSortField sortField)
        {
            switch (sortField)
            {
                case GetOnlineUsersSortField.ConnectionTime:
                    Array.Sort(infos, sortType == SortType.Asc ?
                        new Comparison<UserConnectionInfo>((x, y) => x.ConnectionTime.CompareTo(y.ConnectionTime)) :
                        new Comparison<UserConnectionInfo>((x, y) => y.ConnectionTime.CompareTo(x.ConnectionTime)));

                    break;
            }
        }
    }
}
