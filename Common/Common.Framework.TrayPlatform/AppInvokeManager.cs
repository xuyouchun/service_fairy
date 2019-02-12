using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Package.Cache;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Contracts.Service;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// Server Info 管理器
    /// </summary>
    public class AppInvokeManager
    {
        public AppInvokeManager()
        {
            
        }

        private AppInvokeInfoCollection _serviceInfos = new AppInvokeInfoCollection(new AppInvokeInfo[0]);
        private AppInvokeInfo[] _invokeInfos = new AppInvokeInfo[0];

        /// <summary>
        /// 更新调用列表
        /// </summary>
        /// <param name="invokeInfos"></param>
        public void Update(AppInvokeInfo[] invokeInfos)
        {
            Contract.Requires(invokeInfos != null);

            _invokeInfos = invokeInfos;
            _serviceInfos = new AppInvokeInfoCollection(invokeInfos);
        }

        /// <summary>
        /// 获取调用列表
        /// </summary>
        /// <returns></returns>
        public AppInvokeInfo[] Get()
        {
            return _invokeInfos;
        }

        /// <summary>
        /// 根据服务名称获取Server信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public AppInvokeInfo GetServerInfoByServiceName(string serviceName, SVersion version)
        {
            Contract.Requires(serviceName != null);

            return _serviceInfos.GetServerInfoByServiceName(serviceName, version);
        }

        /// <summary>
        /// 根据服务名称获取各个版本的Server信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public IDictionary<SVersion, AppInvokeInfo> GetServerInfosByServiceName(string serviceName)
        {
            Contract.Requires(serviceName != null);

            return _serviceInfos.GetServerInfosByServiceName(serviceName);
        }
    }
}
