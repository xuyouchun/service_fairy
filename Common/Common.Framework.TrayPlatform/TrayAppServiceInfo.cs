using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using System.Reflection;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// TrayAppService的信息
    /// </summary>
    [Serializable]
    public class TrayAppServiceInfo : MarshalByRefObjectEx
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal TrayAppServiceInfo(AppDomain appDomain, string assemblyFile)
        {
            AppDomain = appDomain;
            AssemblyFile = assemblyFile;
            StartTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime StartTime { get; internal set; }

        /// <summary>
        /// 服务的信息
        /// </summary>
        public AppServiceInfo AppServiceInfo { get; internal set; }

        /// <summary>
        /// 程序集
        /// </summary>
        public string AssemblyFile { get; internal set; }

        /// <summary>
        /// 当前是否为有效状态
        /// </summary>
        public bool Avaliable { get; set; }

        /// <summary>
        /// 服务
        /// </summary>
        public IAppService Service { get; internal set; }

        /// <summary>
        /// 所属的AppDomain
        /// </summary>
        internal AppDomain AppDomain { get; set; }

        /// <summary>
        /// 回调所使用的通信方式
        /// </summary>
        internal ICommunicateFactory CommunicateFactory { get; set; }

        /// <summary>
        /// 配置信息管理器
        /// </summary>
        internal ITrayConfiguration Configuration { get; set; }

        /// <summary>
        /// Cookie管理器
        /// </summary>
        internal ITrayCookieManager CookieManager { get; set; }

        /// <summary>
        /// 平台管理器
        /// </summary>
        internal ITrayPlatform Platform { get; set; }

        /// <summary>
        /// 代理管理器
        /// </summary>
        internal ITrayProxyManager ProxyManager { get; set; }

        /// <summary>
        /// 日志管理器
        /// </summary>
        internal ITrayLogManager LogManager { get; set; }

        /// <summary>
        /// 会话状态管理器
        /// </summary>
        internal ITraySessionStateManager SessionStateManager { get; set; }

        /// <summary>
        /// 缓存管理器
        /// </summary>
        internal ITrayCacheManager TrayCacheManager { get; set; }

        /// <summary>
        /// 线程同步器
        /// </summary>
        internal ITraySynchronizer Synchronizer { get; set; }

        /// <summary>
        /// 服务状态
        /// </summary>
        public AppServiceStatus Status { get; internal set; }

        /// <summary>
        /// 运行时信息
        /// </summary>
        public AppServiceRuntimeInfo RuntimeInfo { get; set; }

        internal void DisposeAll()
        {
            HashSet<object> hs = new HashSet<object> { this };
            foreach (object obj in _GetAllObjects())
            {
                IDisposable dis = obj as IDisposable;
                if (dis == null || !hs.Add(obj))
                    continue;

                try
                {
                    dis.Dispose();
                }
                catch (Exception ex)
                {
                    Common.Package.LogManager.LogError(ex);
                }
            }
        }

        private IEnumerable<object> _GetAllObjects()
        {
            return new object[] {
                CommunicateFactory, Configuration, CookieManager, Platform, ProxyManager, LogManager, SessionStateManager,
                TrayCacheManager, Synchronizer,
            };
        }

        /// <summary>
        /// 从AppServiceInfo中转换
        /// </summary>
        /// <returns></returns>
        public ServiceInfo ToServiceInfo()
        {
            TrayAppServiceInfo t = this;
            AppServiceInfo s = t.AppServiceInfo;
            return new ServiceInfo() {
                ServiceDesc = s.ServiceDesc, Status = t.Status, Title = s.Title, Desc = s.Desc, StartTime = t.StartTime,
                RuntimeInfo = t.RuntimeInfo, Avaliable = t.Avaliable,
            };
        }
    }
}
