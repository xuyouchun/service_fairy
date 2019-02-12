using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.Service;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 云平台管理器
    /// </summary>
    [AppComponent("云平台管理器", "获取云平台的运行状况、读取信息等", AppComponentCategory.System, "Sys_CloudManager")]
    public class CloudManagerAppComponent : AppComponent
    {
        public CloudManagerAppComponent(CoreAppServiceBase service)
            : base(service)
        {
            _service = service;
            _allClientIds = new AutoLoad<Guid[]>(() => _service.Invoker.Deploy.GetAllClientIds(), TimeSpan.FromSeconds(2));
        }

        private readonly CoreAppServiceBase _service;
        private readonly Cache<ServiceDesc, AppInvokeInfo[]> _serviceCache = new Cache<ServiceDesc, AppInvokeInfo[]>();
        private readonly AutoLoad<Guid[]> _allClientIds;

        /// <summary>
        /// 搜索指定服务的部署集团
        /// </summary>
        /// <param name="serviceDesc">服务</param>
        /// <returns></returns>
        public AppInvokeInfo[] SearchServices(ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);

            return _serviceCache.GetOrAddOfRelative(serviceDesc, TimeSpan.FromSeconds(2), (key) => {
                return _service.Invoker.Deploy.SearchServices(serviceDesc);
            });
        }

        /// <summary>
        /// 获取所有服务终端的唯一标识
        /// </summary>
        /// <returns></returns>
        public Guid[] GetAllClientIds()
        {
            return _allClientIds.Value;
        }
    }
}
