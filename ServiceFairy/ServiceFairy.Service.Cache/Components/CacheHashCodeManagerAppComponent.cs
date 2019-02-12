using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common;

namespace ServiceFairy.Service.Cache.Components
{
    /// <summary>
    /// 管理缓存键的哈希码，用于确定哪些key在哪些服务上存在
    /// </summary>
    class CacheHashCodeManagerAppComponent : TimerAppComponentBase
    {
        public CacheHashCodeManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {

        }

        private readonly Dictionary<Guid, HashSet<int>> _dict = new Dictionary<Guid, HashSet<int>>();
        private readonly RwLocker _locker = new RwLocker();

        protected override void OnExecuteTask()
        {
            
        }




    }
}
