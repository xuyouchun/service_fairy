using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common;

namespace ServiceFairy
{
    public class ServiceFairyAssemblySettings
    {
        public ServiceFairyAssemblySettings(ServiceFairyAssemblyInfo[] assemblyInfos, AppClientInitInfo initInfo)
        {
            Contract.Requires(assemblyInfos != null);

            AssemblyInfos = assemblyInfos;
            InitInfo = initInfo;
        }

        /// <summary>
        /// 程序集配置信息
        /// </summary>
        public ServiceFairyAssemblyInfo[] AssemblyInfos { get; private set; }

        /// <summary>
        /// 初始化信息
        /// </summary>
        public AppClientInitInfo InitInfo { get; private set; }

        public static readonly ServiceFairyAssemblySettings Empty = new ServiceFairyAssemblySettings(Array<ServiceFairyAssemblyInfo>.Empty, AppClientInitInfo.Empty);
    }

    /// <summary>
    /// 服务终端的描述信息
    /// </summary>
    [Serializable]
    public class AppClientInitInfo
    {
        public AppClientInitInfo(Guid clientId, Guid platformDeployId, string runningPath = null,
            string servicePath = null, string deployPackagePath = null, string dataPath = null, string logPath = null, string clientTitle = "", string clientDesc = "")
        {
            ClientID = clientId;
            PlatformDeployID = platformDeployId;
            RunningPath = runningPath ?? SFSettings.DefaultRunningPath;
            ServicePath = servicePath ?? SFSettings.DefaultServicePath;
            DeployPackagePath = deployPackagePath ?? SFSettings.DefaultDeployPackagePath;
            DataPath = dataPath ?? SFSettings.DefaultDataPath;
            LogPath = logPath ?? SFSettings.DefaultLogPath;
            
            ClientTitle = clientTitle;
            ClientDesc = clientDesc;
        }

        /// <summary>
        /// 客户端唯一标识
        /// </summary>
        public Guid ClientID { get; private set; }

        /// <summary>
        /// 平台部署版本
        /// </summary>
        public Guid PlatformDeployID { get; private set; }

        /// <summary>
        /// 运行路径
        /// </summary>
        public string RunningPath { get; private set; }

        /// <summary>
        /// 服务路径
        /// </summary>
        public string ServicePath { get; private set; }

        /// <summary>
        /// 安装包路径
        /// </summary>
        public string DeployPackagePath { get; private set; }

        /// <summary>
        /// 数据路径
        /// </summary>
        public string DataPath { get; private set; }

        /// <summary>
        /// 日志路径
        /// </summary>
        public string LogPath { get; private set; }

        /// <summary>
        /// 客户端标题
        /// </summary>
        public string ClientTitle { get; private set; }

        /// <summary>
        /// 客户端描述
        /// </summary>
        public string ClientDesc { get; private set; }

        public static readonly AppClientInitInfo Empty = new AppClientInitInfo(Guid.Empty, Guid.Empty, "");
    }
}
