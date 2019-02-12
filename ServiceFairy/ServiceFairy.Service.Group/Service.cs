using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.Group.Components;
using ServiceFairy.Entities.Group;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 群组服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Group, "1.0", "群组服务",
        category: AppServiceCategory.System, desc: "维护群组信息，并维持各个群组成员之间的会话")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                GroupAccountManager = new GroupAccountManagerAppComponent(this),
                GroupMessageDispatcher = new GroupMessageDispatcherAppComponent(this),
            });

            this.LoadStatusCodeInfosFromType(typeof(GroupStatusCode));
        }

        /// <summary>
        /// 群组帐号管理器
        /// </summary>
        public GroupAccountManagerAppComponent GroupAccountManager { get; private set; }

        /// <summary>
        /// 群组消息分发器
        /// </summary>
        public GroupMessageDispatcherAppComponent GroupMessageDispatcher { get; private set; }
    }
}
