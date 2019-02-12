using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Components;
using Common.Contracts.Service;
using ServiceFairy.Service.UserCenter.Components;
using Common;
using Common.Utility;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 用户中心服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.UserCenter, "1.0",
        "用户中心", category: AppServiceCategory.System, weight: 30, desc:"维护用户的在线状态")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                UserConnectionManager = new UserConnectionManagerAppComponent(this),
                UserRouter = new UserRouterAppComponent(this),
                RoutableUserConnectionManager = new RoutableUserConnectionManagerAppComponent(this),
                UserRelationManager = new UserRelationManagerAppComponent(this),
                UserStatusManager = new UserStatusManagerAppComponent(this),
                UserCollectionParser = new UserCollectionParserAppComponent(this),
                UserListManager = new UserListManagerAppComponent(this),
                UserInfoManager = new UserInfoManagerAppComponent(this),
                GroupSessionStateManager = new GroupSessionStateManagerAppComponent(this),
                GroupInfoManager = new GroupInfoManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 保持用户连接状态
        /// </summary>
        public UserConnectionManagerAppComponent UserConnectionManager { get; private set; }

        /// <summary>
        /// 支持路由功能的用户状态管理器
        /// </summary>
        public RoutableUserConnectionManagerAppComponent RoutableUserConnectionManager { get; private set; }

        /// <summary>
        /// 用户路由器
        /// </summary>
        public UserRouterAppComponent UserRouter { get; private set; }

        /// <summary>
        /// 用户关系管理器
        /// </summary>
        public UserRelationManagerAppComponent UserRelationManager { get; private set; }

        /// <summary>
        /// 用户状态信息管理器
        /// </summary>
        public UserStatusManagerAppComponent UserStatusManager { get; private set; }

        /// <summary>
        /// 用户集合解析器
        /// </summary>
        public UserCollectionParserAppComponent UserCollectionParser { get; private set; }

        /// <summary>
        /// 用户列表管理器
        /// </summary>
        public UserListManagerAppComponent UserListManager { get; private set; }

        /// <summary>
        /// 用户信息管理器
        /// </summary>
        public UserInfoManagerAppComponent UserInfoManager { get; private set; }

        /// <summary>
        /// 群组状态管理器
        /// </summary>
        public GroupSessionStateManagerAppComponent GroupSessionStateManager { get; private set; }

        /// <summary>
        /// 群组信息管理器
        /// </summary>
        public GroupInfoManagerAppComponent GroupInfoManager { get; set; }
    }
}
