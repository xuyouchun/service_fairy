using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Components;

namespace ServiceFairy.Service.UserCenter.Components
{
    /// <summary>
    /// 用户路由器
    /// </summary>
    [AppComponent("用户路由器", "用于确定用户在哪个用户中心服务上注册")]
    class UserRouterAppComponent : ServiceConsistentNodeManagerAppComponent
    {
        public UserRouterAppComponent(Service service)
            : base(service, service.Context.ServiceDesc)
        {

        }

        /// <summary>
        /// 获取用户所在的终端
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid GetClientId(int userId)
        {
            return base.GetNode(userId).ClientID;
        }
    }
}
