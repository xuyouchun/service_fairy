using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.User.Components;
using ServiceFairy.Entities.User;
using ServiceFairy.Entities;
using Common.Utility;

namespace ServiceFairy.Service.User
{
    [AppEntryPoint, AppService(SFNames.ServiceNames.User, "1.0", "用户服务",
        category: AppServiceCategory.System, weight: 30, desc:"用户注册、登录等帐号管理功能，及状态变化、用户信息下载等")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                UserAccountManager = new UserAccountManagerAppComponent(this),
                MobileUserAccountManager = new MobileUserAccountManagerAppComponent(this),
                UserStateManager = new UserStatusManagerAppComponent(this),
                UserMessageDispatcher = new UserMessageDispatcherAppComponent(this),
                ContactListManager = new ContactListManagerAppComponent(this),
                UserRelationManager = new UserRelationManagerAppComponent(this),
            });

            this.LoadStatusCodeInfosFromType(typeof(UserStatusCode));
        }

        /// <summary>
        /// 用户管理器
        /// </summary>
        public UserAccountManagerAppComponent UserAccountManager { get; private set; }

        /// <summary>
        /// 手机用户管理器
        /// </summary>
        public MobileUserAccountManagerAppComponent MobileUserAccountManager { get; private set; }

        /// <summary>
        /// 用户状态管理器
        /// </summary>
        public UserStatusManagerAppComponent UserStateManager { get; private set; }

        /// <summary>
        /// 用户消息管理器
        /// </summary>
        public UserMessageDispatcherAppComponent UserMessageDispatcher { get; private set; }

        /// <summary>
        /// 通信录管理器
        /// </summary>
        public ContactListManagerAppComponent ContactListManager { get; private set; }

        /// <summary>
        /// 用户关联关系管理器
        /// </summary>
        public UserRelationManagerAppComponent UserRelationManager { get; private set; }
    }
}

