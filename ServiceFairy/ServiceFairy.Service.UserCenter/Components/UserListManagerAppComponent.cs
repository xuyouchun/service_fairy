using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.UserCenter;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Service.UserCenter.Components
{
    /// <summary>
    /// 用户列表管理器
    /// </summary>
    [AppComponent("用户列表管理器", "提供已注册的用户列表")]
    class UserListManagerAppComponent : TimerAppComponentBase
    {
        public UserListManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {

        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }

        /// <summary>
        /// 获取已注册的用户列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public UserIdName[] GetUserList(int start, int count, string order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取已注册的用户总数量
        /// </summary>
        /// <returns></returns>
        public int GetUserCount()
        {
            return 0;
        }
    }
}
