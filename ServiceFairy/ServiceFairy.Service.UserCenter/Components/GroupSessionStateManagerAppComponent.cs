using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;
using Common.Package;
using ServiceFairy.SystemInvoke;
using Common.Utility;
using ServiceFairy.Components;

namespace ServiceFairy.Service.UserCenter.Components
{
    /// <summary>
    /// 群组状态管理器
    /// </summary>
    [AppComponent("群组状态管理器", "管理群组的会话状态")]
    class GroupSessionStateManagerAppComponent : AppComponent
    {
        public GroupSessionStateManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;

            _cache.AddWeakReferenceCache();
            _cache.AddDistributedCache(service.Invoker, TimeSpan.FromMinutes(30), "s_g_");
            _cache.AddLoader(_LoadGroupSessionState);
        }

        private readonly Service _service;
        private readonly CacheChain<int, GroupSessionState> _cache = new CacheChain<int, GroupSessionState>();

        /// <summary>
        /// 获取用户的SessionState
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public GroupSessionState GetGroupSessionState(int groupId)
        {
            return _cache.Get(groupId);
        }

        /// <summary>
        /// 获取用户的SessionState
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public GroupSessionState[] GetGroupSessionState(int[] groupIds)
        {
            return _cache.GetRange(groupIds).ToArray(g => g.Value);
        }

        /// <summary>
        /// 根据SessionState
        /// </summary>
        /// <param name="groupIds"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        private KeyValuePair<int, GroupSessionState>[] _LoadGroupSessionState(int[] groupIds, bool refresh)
        {
            return null;
        }
    }
}
