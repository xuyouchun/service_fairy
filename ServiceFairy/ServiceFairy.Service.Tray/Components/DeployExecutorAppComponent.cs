using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Common.Collection;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using Common.Package;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Configuration;
using ServiceFairy.Entities.Deploy;
using ServiceFairy.Entities.Master;
using ServiceFairy.Install;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Service.Tray.Components
{
    /// <summary>
    /// 升级与部署
    /// </summary>
    [AppComponent("升级与部署", "从部署服务及配置服务下载最新的程序集及配置，并更新至本地")]
    class DeployExecutorAppComponent : AppComponent
    {
        public DeployExecutorAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;
        private readonly Dictionary<ServiceDesc, AppServiceConfiguration> _serviceConfigVerDict = new Dictionary<ServiceDesc, AppServiceConfiguration>();

        /// <summary>
        /// 下载与更新最新的配置文件
        /// </summary>
        /// <param name="verDict"></param>
        public bool DownloadAndUpdateConfiguration(Dictionary<ServiceDesc, DateTime> verDict)
        {
            bool modified = false;
            _serviceConfigVerDict.ToDictionary(v => v.Key, v => v.Value.LastUpdate, true).CompareTo(verDict,
                delegate(ServiceDesc sd, DateTime oldVer, DateTime newVer, CollectionChangedType changeType) {
                    if (changeType == CollectionChangedType.Remove)
                    {
                        modified = true;
                        _serviceConfigVerDict.Remove(sd);
                    }
                    else if (changeType != CollectionChangedType.NoChange)
                    {
                        modified = true;
                        AutoRetry(delegate {
                            ServiceResult<Configuration_DownloadConfiguration_Reply> sr = ConfigurationService.DownloadConfiguration(_service.Context.ServiceClient,
                                new Configuration_DownloadConfiguration_Request() { ServiceDesc = sd, LastUpdate = oldVer });

                            if (sr.Succeed)
                            {
                                if (sr.Result == null || sr.Result.Configuration == null || sr.Result.Configuration.LastUpdate < newVer)
                                    return false;

                                string content = sr.Result.Configuration.Content;
                                _service.Context.Configuration.UpdateConfiguration(sd, content);
                                _serviceConfigVerDict[sd] = new AppServiceConfiguration() { LastUpdate = newVer, Content = content };
                                //PathUtility.WriteAllText(ServiceFairyUtility.GetRunningConfigPathOfService(sd, _service.RunningPath, _service.ClientID), content, Encoding.UTF8);
                                return true;
                            }

                            return false;
                        }, 2);  // 两次重试
                    }
                });

            return modified;
        }

        private Guid _lastPlatformDeployPackageId;

        /// <summary>
        /// 下载与部署AppService
        /// </summary>
        /// <param name="deployVer"></param>
        /// <returns></returns>
        public bool? DownloadAndUpdateAppServicesAndPlatform(DateTime deployVer)
        {
            SystemInvoker invoker = SystemInvoker.FromServiceClient(_service.Context.ServiceClient);
            ServiceResult<AppClientDeployInfo> sr = invoker.Deploy.GetDeployMapSr(_service.Context.ClientID);
            AppClientDeployInfo dInfo;
            if (!sr.Succeed || (dInfo = sr.Result) == null || dInfo.LastUpdate < deployVer)
                return null;

            // 与本地平台版本号对比，如果不同，则下载最新的平台并重新启动
            if (dInfo.PlatformDeployPackageId != Guid.Empty)
            {
                try
                {
                    Guid platformId = _service.Context.PlatformDeployId;
                    if ((platformId == Guid.Empty || platformId != dInfo.PlatformDeployPackageId) && _lastPlatformDeployPackageId != dInfo.PlatformDeployPackageId)
                    {
                        _DownloadPlatformPackage(dInfo.PlatformDeployPackageId);
                        _lastPlatformDeployPackageId = dInfo.PlatformDeployPackageId;  // 防止重复下载
                        _service.Context.Platform.DoCommand("LiveUpdate", new[] { "/pkgId:" + dInfo.PlatformDeployPackageId.ToString() });

                        return null;
                    }
                }
                catch (ThreadAbortException) { }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }

            bool changed = false;

            if (dInfo.InvokeInfos != null)  // 其它Service的调用地址
                _service.Context.Platform.UpdateInvokeInfos(dInfo.InvokeInfos);

            if (dInfo.CommunicateOptions != null && _OpenOrCloseCommunicateOptions(dInfo.CommunicateOptions))  // 需要开启或关闭的通信方式
                changed = true;

            if (dInfo.Services != null && _StartOrStopServices(dInfo.Services.ToArray(v => v.ServiceDesc))) // 开启或停止的服务
                changed = true;

            return changed;
        }

        // 下载平台安装包
        private void _DownloadPlatformPackage(Guid deployPackageId)
        {
            PlatformDeployPackageInfo packageInfo = _service.Invoker.Master.GetPlatformDeployPackageInfo(deployPackageId);
            DeployPackage deployPackage;
            if (packageInfo == null || (deployPackage = _service.Invoker.Master.DownloadPlatformDeployPackage(deployPackageId)) == null)
                throw new ServiceException(ServerErrorCode.NoData, "平台安装包不存在：" + deployPackageId);

            string liveUpdatePath = Path.Combine(PathUtility.GetExecutePath(), SFSettings.PlatformLiveUpdateDirectory + "\\Update");
            PathUtility.ClearPath(liveUpdatePath);

            string path = Path.Combine(liveUpdatePath, PathUtility.GetUtcTimePath());
            PathUtility.CreateDirectoryIfNotExists(path);

            StreamUtility.DecompressToDirectory(deployPackage.Content, path);
            PlatformDeployPackageInfo.SerializeToFile(packageInfo, Path.Combine(path, SFSettings.PlatformDeployPackageInfoFile));
        }

        /// <summary>
        /// 开启或关闭通信方式
        /// </summary>
        /// <param name="communicationOption"></param>
        private bool _OpenOrCloseCommunicateOptions(CommunicationOption[] newOptions)
        {
            bool changed = false;
            List<CommunicationOption> closeList = new List<CommunicationOption>(), openList = new List<CommunicationOption>();
            CommunicationOption[] curOps = _service.Context.Platform.GetAllCommunicationOptions();
            curOps.CompareTo(newOptions, delegate(CommunicationOption op, CollectionChangedType changedType) {
                if (changedType == CollectionChangedType.Add || changedType == CollectionChangedType.Remove)
                {
                    changed = true;
                    (changedType == CollectionChangedType.Add ? openList : closeList).Add(op);
                }
            });

            foreach (CommunicationOption op in closeList)
            {
                _service.Context.Platform.StopListener(op.Address);
            }

            foreach (CommunicationOption op in openList)
            {
                _service.Context.Platform.StartListener(op);
            }

            return changed;
        }

        /// <summary>
        /// 开启或关闭AppService
        /// </summary>
        /// <param name="newSds"></param>
        /// <returns>是否有所改变</returns>
        private bool _StartOrStopServices(ServiceDesc[] newSds)
        {
            bool changed = false;
            _service.Context.Platform.GetAllServiceInfos().ToArray(si => si.ServiceDesc).CompareTo(newSds, delegate(ServiceDesc sd, CollectionChangedType changedType) {
                if (changedType == CollectionChangedType.Remove)
                {
                    if (sd != _service.Context.ServiceDesc)
                    {
                        _UnloadService(sd);
                        changed = true;
                    }
                }
                else if (changedType == CollectionChangedType.Add)
                {
                    _TryLoadService(sd);
                    changed = true;
                }
            });

            return changed;
        }

        private bool _UnloadService(ServiceDesc sd)
        {
            try
            {
                _service.Context.Platform.UnloadService(sd);
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                LogManager.LogError(ex);
                return false;
            }
        }

        private bool _TryLoadService(ServiceDesc sd)
        {
            if (_errorServiceDescs.Get(sd) != null)
                return false;

            try
            {
                return _LoadService(sd, true);
            }
            catch (Exception ex)
            {
                _errorServiceDescs.AddOfDynamic(sd, new object(), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5));
                LogManager.LogError(ex);
                return false;
            }
        }

        private readonly Cache<ServiceDesc, object> _errorServiceDescs = new Common.Package.Cache<ServiceDesc, object>();

        private bool _LoadService(ServiceDesc sd, bool autoStart)
        {
            string path = _DownloadPackage(sd);
            string configuration = _DownloadConfiguration(sd);

            if (!string.IsNullOrEmpty(path))
            {
                _service.Context.Platform.LoadService(path, configuration ?? string.Empty, autoStart);
                return true;
            }

            return false;
        }

        private string _DownloadPackage(ServiceDesc sd)
        {
            string assemblyFile = SFSettings.GetRunningMainAssemblyPath(sd, _service.RunningBasePath, _service.Context.ClientID);
            /*if (File.Exists(assemblyFile))
                return assemblyFile;*/  // 每次都下载最新的程序集来运行

            // 下载程序集
            ServiceResult<Deploy_DownloadDeployPackage_Reply> sr = DeployService.DownloadDeployPackage(
                _service.Context.ServiceClient, new Deploy_DownloadDeployPackage_Request() { ServiceDesc = sd });

            if (sr.Succeed)
            {
                if (sr.Result != null && sr.Result.DeployPackage != null)
                {
                    string runningPath = SFSettings.GetServiceRunningPath(sd, _service.RunningBasePath, _service.Context.ClientID);
                    string tempPath = Path.Combine(Path.GetTempPath(), "ServiceDeployPackage", sd + "_" + Guid.NewGuid() + ".tmp");
                    string servicePath = SFSettings.GetServicePath(sd, _service.ServiceBasePath);

                    // 先解压到临时目录，再判断版本是否已经升级，如果未升级则使用本地的版本
                    StreamUtility.DecompressToDirectory(sr.Result.DeployPackage.Content, tempPath);
                    bool upgraded = _GetServicePackageLastUpdate(tempPath) >= _GetServicePackageLastUpdate(servicePath);
                    PathUtility.CopyDirectory(upgraded ? tempPath : servicePath, runningPath, true);
                    PathUtility.DeleteIfExists(tempPath);
                    
                    if (!File.Exists(assemblyFile))
                        throw new ServiceException(ServerErrorCode.DataError, string.Format("错误的安装包格式，未找到服务“{0}”的Main.dll", sd));

                    return assemblyFile;
                }
                else
                    throw new ServiceException(ServerErrorCode.DataError);
            }
            else
            {
                if (sr.StatusCode != (int)ServerErrorCode.DataNotReady)
                    throw sr.CreateException();

                return null;
            }
        }

        private DateTime _GetServicePackageLastUpdate(string servicePath)
        {
            ServiceDeployPackageSettings settings = ServiceDeployPackageSettings.TryLoad(Path.Combine(servicePath, ServiceDeployPackageSettings.DefaultFileName));
            return (settings != null) ? settings.LastUpdate : default(DateTime);
        }

        // 下载配置文件
        private string _DownloadConfiguration(ServiceDesc sd)
        {
            AppServiceConfiguration cfg;
            if (_serviceConfigVerDict.TryGetValue(sd, out cfg))
                return cfg.Content;

            ServiceResult<Configuration_DownloadConfiguration_Reply> sr = ConfigurationService.DownloadConfiguration(_service.Context.ServiceClient,
                new Configuration_DownloadConfiguration_Request { LastUpdate = default(DateTime), ServiceDesc = sd });

            if (sr.Succeed)
            {
                return (sr.Result != null && sr.Result.Configuration != null) ? sr.Result.Configuration.Content : "";
            }

            throw sr.CreateException();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceDesc"></param>
        public bool LoadService(ServiceDesc serviceDesc)
        {
            return _LoadService(serviceDesc, true);
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _errorServiceDescs.Dispose();
        }
    }
}
