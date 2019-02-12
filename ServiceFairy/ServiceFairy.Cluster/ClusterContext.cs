using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Cluster.Components;
using ServiceFairy.SystemInvoke;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Communication.Wcf;
using Common.Contracts;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Cluster
{
    /// <summary>
    /// 集群的上下文环境
    /// </summary>
    public class ClusterContext : IDisposable
    {
        public ClusterContext(CommunicationOption communicationOption, string servicePath, string runningPath, string deployPackagePath)
        {
            Contract.Requires(communicationOption != null && servicePath != null && runningPath != null && deployPackagePath != null);

            ServicePath = servicePath;
            RunningPath = runningPath;
            DeployPackagePath = deployPackagePath;
            MasterServiceCommunicationOption = communicationOption;
            _systemInvoker = new Lazy<SystemInvoker>(delegate {
                WcfService wcfService = new WcfService();
                WcfConnection con = wcfService.Connect(communicationOption.Address, communicationOption.CommunicationType, communicationOption.SupportDuplex);
                con.Open();
                _disposableObjects.AddRange(new IDisposable[] { con, wcfService });
                return new SystemInvoker(new ServiceFairyClient(con, DataFormat.Binary, BufferType.Bytes));
            });
        }

        public ClusterContext(SystemInvoker systemInvoker, string basePath)
        {
            Contract.Requires(systemInvoker != null && basePath != null);

            RunningPath = basePath;
            _systemInvoker = new Lazy<SystemInvoke.SystemInvoker>(() => systemInvoker);
        }

        /// <summary>
        /// 服务器端连接
        /// </summary>
        public CommunicationOption MasterServiceCommunicationOption { get; private set; }

        /// <summary>
        /// 服务路径
        /// </summary>
        public string ServicePath { get; private set; }

        /// <summary>
        /// 运行路径
        /// </summary>
        public string RunningPath { get; private set; }

        /// <summary>
        /// 安装包路径
        /// </summary>
        public string DeployPackagePath { get; private set; }

        private Lazy<SystemInvoker> _systemInvoker;

        public SystemInvoker SystemInvoker
        {
            get
            {
                return _systemInvoker.Value;
            }
        }

        private readonly List<IDisposable> _disposableObjects = new List<IDisposable>();

        /// <summary>
        /// 组件管理器
        /// </summary>
        public ClusterComponentManager ComponentManager { get; set; }

        public void Dispose()
        {
            _disposableObjects.ForEach(v => v.Dispose());

            if (ComponentManager != null)
                ComponentManager.Dispose();
        }
    }
}
