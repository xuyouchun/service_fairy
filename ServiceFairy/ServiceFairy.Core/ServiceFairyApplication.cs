using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Utility;
using Common.Framework;
using System.IO;
using Common.Package;
using System.Diagnostics;
using Common;
using Common.Framework.TrayPlatform;
using System.Threading;
using Common.Contracts.Log;
using Common.Package.TaskDispatcher;
using Common.Communication.Wcf;

namespace ServiceFairy
{
    /// <summary>
    /// Service Fairy 的运行类
    /// </summary>
    public class ServiceFairyApplication : WcfTrayAppServiceApplication, IExecutableAssembly
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="basePath"></param>
        /// <param name="clientId"></param>
        public ServiceFairyApplication(IServiceProvider sp)
        {
            Contract.Requires(sp != null);
            _sp = sp;

            LogManager.RegisterLogWritter(base.TrayLogManager.Writer);
            LogManager.LogMessage("系统开始启动 ...");
        }

        protected override WcfConnection OnCreateConnection(string serviceName, SVersion version)
        {
            if (serviceName == SFNames.ServiceNames.Master)
            {
                CommunicationOption op = _GetMasterCommunicationOption();
                if (op != null)
                    return WcfService.Connect(op.Address, op.Type, op.Duplex);
            }

            return base.OnCreateConnection(serviceName, version);
        }

        private CommunicationOption _GetMasterCommunicationOption()
        {
            object data = ServiceManager.GetData(SFNames.DataKeys.MASTER_COMMUNICATION);
            CommunicationOption[] ops = data as CommunicationOption[];
            if (ops != null)
                return TrayUtility.PickCommunicationOption(ops);

            return data as CommunicationOption;
        }

        private readonly IServiceProvider _sp;

        protected override void OnLoadServices()
        {
            base.OnLoadServices();

            IObjectProvider<ServiceFairyAssemblySettings> sfSettingsProvider = _sp.GetService<IObjectProvider<ServiceFairyAssemblySettings>>(true);

            ServiceFairyAssemblySettings sfSettings = sfSettingsProvider.Get();
            AppClientInitInfo initInfo = sfSettings.InitInfo;
            ServiceManager.SetData(SystemDataKeys.CLIENT_ID, initInfo.ClientID);
            ServiceManager.SetData(SystemDataKeys.PLATFORM_DEPLOY_ID, initInfo.PlatformDeployID);
            ServiceManager.SetData(SFNames.DataKeys.RUNNING_BASE_PATH, initInfo.RunningPath);
            ServiceManager.SetData(SFNames.DataKeys.SERVICE_BASE_PATH, initInfo.ServicePath);
            ServiceManager.SetData(SFNames.DataKeys.DEPLOY_PACKAGE_PATH, initInfo.DeployPackagePath);
            ServiceManager.SetData(SFNames.DataKeys.DATA_PATH, initInfo.DataPath);
            ServiceManager.SetData(SFNames.DataKeys.INSTALL_PATH, PathUtility.GetExecutePath());
            ServiceManager.SetData(SFNames.DataKeys.LOG_PATH, initInfo.LogPath);
            ServiceManager.SetData(SFNames.DataKeys.CLIENT_TITLE, initInfo.ClientTitle);
            ServiceManager.SetData(SFNames.DataKeys.CLIENT_DESC, initInfo.ClientDesc);

            TrayAppServiceInfo[] sInfos = _LoadAppServiceInfos(sfSettings);
            _StartServiceInfos(sInfos);
        }

        private void _StartServiceInfos(TrayAppServiceInfo[] sInfos)
        {
            int succeedCount = 0, failedCount = 0;
            using (TaskDispatcher<StartServiceTask> dispatcher = new TaskDispatcher<StartServiceTask>(5))
            {
                dispatcher.TaskCompleted += delegate(object sender, TaskCompletedEventArgs<StartServiceTask> e) {
                    if (e.Task.Succeed) succeedCount++;
                    else failedCount++;
                };
                dispatcher.AddRange(sInfos.Select(si => new StartServiceTask(si)));
                dispatcher.WaitForAllCompleted();
            }

            string s = string.Format("启动完毕，共{0}个服务成功启动", succeedCount);
            if (failedCount > 0) s += string.Format("，{0}个失败", failedCount);
            LogManager.LogMessage(s);
        }

        #region Class StartServiceTask ...

        class StartServiceTask : ITask
        {
            public StartServiceTask(TrayAppServiceInfo sInfo)
            {
                _sInfo = sInfo;
            }

            private readonly TrayAppServiceInfo _sInfo;

            public bool Succeed { get; private set; }

            public void Execute()
            {
                try
                {
                    TrayAppServiceInfo info = ((TrayAppServiceInfo)_sInfo);
                    info.Service.Start();
                    Succeed = true;
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                    Succeed = false;
                }
            }
        }

        #endregion

        // 加载所有服务
        private TrayAppServiceInfo[] _LoadAppServiceInfos(ServiceFairyAssemblySettings sfSettings)
        {
            List<TrayAppServiceInfo> sInfos = new List<TrayAppServiceInfo>();
            int failedCount = 0;
            using (TaskDispatcher<LoadServiceTask> dispatcher = new TaskDispatcher<LoadServiceTask>(5))
            {
                dispatcher.TaskCompleted += delegate(object sender, TaskCompletedEventArgs<LoadServiceTask> e) {
                    if (e.Task.Succeed) sInfos.Add(e.Task.TrayAppServiceInfo);
                    else failedCount++;
                };

                dispatcher.AddRange(sfSettings.AssemblyInfos.Select(ai => new LoadServiceTask(sfSettings, ai, ServiceManager)));
                dispatcher.WaitForAllCompleted();
            }

            string s = string.Format("加载完毕，共{0}个服务成功加载", sInfos.Count);
            if (failedCount > 0) s += string.Format("，{0}个失败", failedCount);
            LogManager.LogMessage(s);
            return sInfos.ToArray();
        }

        #region Class LoadServiceTask ...

        class LoadServiceTask : ITask
        {
            public LoadServiceTask(ServiceFairyAssemblySettings sfSettings, ServiceFairyAssemblyInfo info, TrayAppServiceManager serviceManager)
            {
                _sfSettings = sfSettings;
                _assemblyInfo = info;
                _serviceManager = serviceManager;
            }

            private readonly ServiceFairyAssemblySettings _sfSettings;
            private readonly ServiceFairyAssemblyInfo _assemblyInfo;
            private readonly TrayAppServiceManager _serviceManager;
            public bool Succeed { get; private set; }

            public TrayAppServiceInfo TrayAppServiceInfo { get; private set; }

            public void Execute()
            {
                try
                {
                    ServiceFairyAssemblyInfo info = _assemblyInfo;
                    AppClientInitInfo initInfo = _sfSettings.InitInfo;
                    LogManager.LogMessage("正在加载服务 " + info.ServiceDesc + " ...");

                    ServiceDesc sd = info.ServiceDesc;
                    string runningBasePath = initInfo.RunningPath, serviceBasePath = initInfo.ServicePath, deployPackagePath = initInfo.DeployPackagePath;
                    Guid clientId = initInfo.ClientID;

                    SFSettings.ClearRunningPath(sd, runningBasePath, clientId);
                    string serviceRunningPath = SFSettings.GetServiceRunningPath(sd, runningBasePath, clientId);
                    string servicePath = SFSettings.GetServicePath(sd, serviceBasePath);
                    if (!Directory.Exists(servicePath))
                        throw new DirectoryNotFoundException("不存在服务路径：" + servicePath);

                    PathUtility.CopyDirectoryIfNewer(servicePath, serviceRunningPath, false);

                    string configFile = SFSettings.GetServiceConfigPath(sd, serviceBasePath);
                    string assemblyFile = SFSettings.GetRunningMainAssemblyPath(sd, runningBasePath, clientId);
                    string config = StringUtility.GetFirstNotNullOrWhiteSpaceString(info.Config, PathUtility.ReadAllTextIfExists(configFile));
                    TrayAppServiceInfo = _serviceManager.LoadService(assemblyFile, config);

                    Succeed = true;
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                    Succeed = false;
                }   
            }
        }

        #endregion

        public object Execute(object context, Action<string, string[]> callback, WaitHandle waitHandle)
        {
            try
            {
                Run(callback, waitHandle);
                return Environment.ExitCode;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return -1;
            }
        }
    }
}
