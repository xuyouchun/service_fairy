using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace ServiceFairy
{
    /// <summary>
    /// 需要加载的服务的信息
    /// </summary>
    public class ServiceFairyAssemblyInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceDesc">服务</param>
        /// <param name="config"></param>
        public ServiceFairyAssemblyInfo(ServiceDesc serviceDesc, string config = null)
        {
            Contract.Requires(serviceDesc != null);

            ServiceDesc = serviceDesc;
            Config = config;
        }

        /// <summary>
        /// 服务
        /// </summary>
        public ServiceDesc ServiceDesc { get; private set; }

        /// <summary>
        /// 配置文件内容
        /// </summary>
        public string Config { get; private set; }
    }
}
