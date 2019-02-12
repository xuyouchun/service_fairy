using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 服务的事件
    /// </summary>
    public static class ServiceEventNames
    {
        /// <summary>
        /// 服务终端连接
        /// </summary>
        public const string EVENT_CLIENT_CONNECTED = "EVENT_CLIENT_CONNECTED";

        /// <summary>
        /// 服务终端断开连接
        /// </summary>
        public const string EVENT_CLIENT_DISCONNECTED = "EVENT_CLIENG_DISCONNECTED";

        /// <summary>
        /// 部署地图修改
        /// </summary>
        public const string EVENT_DEPLOY_MAPMODIFIED = "EVENT_DEPLOY_MAPMODIFIED";

        /// <summary>
        /// 新用户注册
        /// </summary>
        public const string EVENT_USER_REGISTER = "EVENT_USER_REGISTER";

        /// <summary>
        /// 用户登录
        /// </summary>
        public const string EVENT_USER_LOGIN = "EVENT_USER_LOGIN";

        /// <summary>
        /// 用户关系发生变化
        /// </summary>
        public const string EVENT_USER_RELATION_CHANGED = "EVENT_USER_RELATION_CHANGED";

        /// <summary>
        /// 用户状态发生变化
        /// </summary>
        public const string EVENT_USER_STATUS_CHANGED = "EVENT_USER_STATUS_CHANGED";

        /// <summary>
        /// 联系人信息发生变化
        /// </summary>
        public const string EVENT_USER_INFO_CHANGED = "EVENT_USER_INFO_CHANGED";

        /// <summary>
        /// 群组信息发生变化
        /// </summary>
        public const string EVENT_GROUP_INFO_CHANGED = "EVENT_GROUP_INFO_CHANGED";

        /// <summary>
        /// 用户上线
        /// </summary>
        public const string EVENT_USERCENTER_ONLINE = "EVENT_USERCENTER_ONLINE";

        /// <summary>
        /// 用户离线
        /// </summary>
        public const string EVENT_USERCENTER_OFFLINE = "EVENT_USERCENTER_OFFLINE";
    }
}
