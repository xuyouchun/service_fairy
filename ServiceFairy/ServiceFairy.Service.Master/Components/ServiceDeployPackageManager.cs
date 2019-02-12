using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package;
using Common.Contracts.Service;
using Common.Utility;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Package.Service;
using ServiceFairy.Entities.Master;
using ServiceFairy.Entities.Deploy;
using Common.Collection;
using System.Runtime.Serialization;
using ServiceFairy.Install;


namespace ServiceFairy.Service.Master.Components
{
    /// <summary>
    /// 安装包管理器
    /// </summary>
    [AppComponent("服务安装包管理器", "上传、下载及部署服务的安装包")]
    partial class ServiceDeployPackageManager : AppComponent
    {
        public ServiceDeployPackageManager(Service service)
            : base(service)
        {
            _service = service;
            _wrapper = new SingleMemoryCache<Wrapper>(TimeSpan.FromSeconds(5), _Load);
        }

        private readonly Service _service;
        private readonly SingleMemoryCache<Wrapper> _wrapper;

        class Wrapper
        {
            public ServiceDeployPackageInfo[] Infos;
            public Dictionary<Guid, ServiceDeployPackageInfo> Dict;
            public Dictionary<ServiceDesc, ServiceDeployPackageInfo[]> ServiceDescDict;
            public Dictionary<ServiceDesc, ServiceDeployPackageInfo> CurrentDict;
        }

        private Wrapper _Load()
        {
            string deployPackagePath = SFSettings.GetServiceDeployPackagePath(_service.DeployPackagePath);

            // 从DeployPackage目录中加载
            ServiceDeployPackageInfo[] infos1 = Directory.GetFiles(deployPackagePath, "*.deployPackageInfo")
                .ToArrayNotNull(file => _TryLoadDeployPackageInfo(file));

            // 从Servie目录中加载
            ServiceDeployPackageInfo[] infos2 = SFSettings.GetAllServiceDescs(_service.ServiceBasePath)
                .ToArrayNotNull(sd => _TryLoadDeployPackageInfo(sd));

            ServiceDeployPackageInfo[] infos = infos1.Union(infos2, ServiceDeployPackageInfo.Comparer).ToArray();
            _firstLoad = false;
            return new Wrapper() {
                Infos = infos,
                Dict = infos.ToDictionary(info => info.Id, true),
                ServiceDescDict = infos.GroupBy(info => info.ServiceDesc).ToDictionary(g => g.Key, g => g.ToArray()),
                CurrentDict = infos2.ToDictionary(info => info.ServiceDesc, true),
            };
        }

        // 从DeployPackage目录加载安装包信息
        private ServiceDeployPackageInfo _TryLoadDeployPackageInfo(string file)
        {
            try
            {
                Guid id;
                if (!File.Exists(file) || !Guid.TryParse(Path.GetFileNameWithoutExtension(file), out id))
                    return null;

                lock (_GetKeyOfDeployPackageId(id))
                {
                    return ServiceDeployPackageInfo.DeserailizeFromFile(file);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                PathUtility.DeleteIfExists(file);
                return null;
            }
        }

        private volatile bool _firstLoad = true;

        // 从Service目录加载安装包信息
        private ServiceDeployPackageInfo _TryLoadDeployPackageInfo(ServiceDesc sd)
        {
            try
            {
                string servicePath = SFSettings.GetServicePath(sd, _service.ServiceBasePath);
                string deployPackageInfoFile = Path.Combine(servicePath, SFSettings.ServiceDeployPackageInfoFile);
                string packagePath = SFSettings.GetServiceDeployPackagePath(_service.DeployPackagePath);

                ServiceDeployPackageInfo info = null;
                if (File.Exists(deployPackageInfoFile))
                {
                    try
                    {
                        info = ServiceDeployPackageInfo.DeserailizeFromFile(deployPackageInfoFile);
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                        PathUtility.DeleteIfExists(deployPackageInfoFile);
                    }
                }

                if (info == null || !File.Exists(_GetDeployPackageInfoFile(info.Id)) || _firstLoad)  // 第一次运行时，都把新的程序集拷贝到package目录中
                {
                    byte[] packageBytes = StreamUtility.CompressDirectory(servicePath);
                    if (info == null)
                    {
                        DateTime lastUpdate = _GetServiceDescLastUpdate(sd);
                        info = new ServiceDeployPackageInfo() {
                            ServiceDesc = sd, Id = Guid.NewGuid(), Title = sd.ToString(),
                            Format = DeployPackageFormat.GZipCompress, LastUpdate = lastUpdate,
                            Size = packageBytes.Length, UploadTime = lastUpdate,
                        };

                        ServiceDeployPackageInfo.SerializeToFile(info, deployPackageInfoFile);
                    }

                    lock (_GetKeyOfDeployPackageId(info.Id))
                    {
                        string path = Path.Combine(packagePath, info.Id.ToString());
                        File.WriteAllBytes(_GetDeployPackageFile(info.Id), packageBytes);
                        File.Copy(deployPackageInfoFile, _GetDeployPackageInfoFile(info.Id), true);
                    }
                }

                lock (_GetKeyOfDeployPackageId(info.Id))
                {
                    string path = Path.Combine(packagePath, info.Id.ToString());
                    return ServiceDeployPackageInfo.DeserailizeFromFile(_GetDeployPackageInfoFile(info.Id));
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return null;
            }
        }

        private DateTime _GetServiceDescLastUpdate(ServiceDesc sd)
        {
            string mainAssemblyFile = SFSettings.GetServiceMainAssemblyPath(sd, _service.ServiceBasePath);
            string settingsFile = Path.Combine(SFSettings.GetServicePath(sd, _service.ServiceBasePath), ServiceDeployPackageSettings.DefaultFileName);
            ServiceDeployPackageSettings settings = ServiceDeployPackageSettings.TryLoad(settingsFile);

            return settings != null ? settings.LastUpdate : File.GetLastWriteTimeUtc(mainAssemblyFile);
        }

        /// <summary>
        /// 获取全部的安装包信息
        /// </summary>
        /// <returns></returns>
        public ServiceDeployPackageInfo[] GetAllInfos()
        {
            return _wrapper.Get().Infos;
        }

        /// <summary>
        /// 获取全部当前安装包信息
        /// </summary>
        /// <returns></returns>
        public ServiceDeployPackageInfo[] GetAllCurrentInfos()
        {
            return _wrapper.Get().CurrentDict.Values.ToArray();
        }

        /// <summary>
        /// 获取安装包
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceDeployPackageInfo GetInfo(Guid id)
        {
            return _wrapper.Get().Dict.GetOrDefault(id);
        }

        /// <summary>
        /// 加载安装包
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        private DeployPackage _LoadDeployPackage(ServiceDesc serviceDesc)
        {
            string path = SFSettings.GetServicePath(serviceDesc, _service.ServiceBasePath);
            if (!Directory.Exists(path))
                return null;

            byte[] content = StreamUtility.CompressDirectory(path);
            return new DeployPackage() {
                Content = content,
            };
        }

        private object _GetKeyOfDeployPackageId(Guid deployPackageId)
        {
            return string.Intern(deployPackageId.ToString().ToLower());
        }

        /// <summary>
        /// 下载安装包
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public DeployPackage Download(ServiceDesc serviceDesc)
        {
            ServiceDeployPackageInfo info = _wrapper.Get().CurrentDict.GetOrDefault(serviceDesc);
            if (info == null)
                return null;

            return Download(info.Id);
        }

        /// <summary>
        /// 下载安装包
        /// </summary>
        /// <param name="deployPackageId"></param>
        /// <returns></returns>
        public DeployPackage Download(Guid deployPackageId)
        {
            string path = Path.Combine(SFSettings.GetServiceDeployPackagePath(_service.DeployPackagePath), deployPackageId.ToString());

            string deployPackageInfoPath = _GetDeployPackageInfoFile(deployPackageId);
            string deployPackagePath = _GetDeployPackageFile(deployPackageId);

            if (!File.Exists(deployPackageInfoPath) || !File.Exists(deployPackagePath))
                return null;

            return new DeployPackage() {
                Content = File.ReadAllBytes(deployPackagePath),
                Format = DeployPackageFormat.GZipCompress,
            };
        }

        /// <summary>
        /// 获取指定安装包的信息
        /// </summary>
        /// <param name="deployPackageId"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public ServiceDeployPackageInfo GetPackageInfo(Guid deployPackageId, bool throwError = false)
        {
            string deployPackageInfoPath = _GetDeployPackageInfoFile(deployPackageId);
            if (File.Exists(deployPackageInfoPath))
            {
                lock (_GetKeyOfDeployPackageId(deployPackageId))
                {
                    return ServiceDeployPackageInfo.DeserailizeFromFile(deployPackageInfoPath);
                }
            }

            if (throwError)
                throw new FileNotFoundException("指定的安装包信息文件不存在：" + deployPackageInfoPath);

            return null;
        }

        /// <summary>
        /// 上传安装包
        /// </summary>
        /// <param name="info"></param>
        /// <param name="content"></param>
        public void Upload(ServiceDeployPackageInfo info, byte[] content)
        {
            Contract.Requires(info != null && content != null);

            if (info.Id == Guid.Empty)
                info.Id = Guid.NewGuid();

            lock (_GetKeyOfDeployPackageId(info.Id))
            {
                ServiceDeployPackageInfo.SerializeToFile(info, _GetDeployPackageInfoFile(info.Id));
                File.WriteAllBytes(_GetDeployPackageFile(info.Id), content);
            }

            string servicePath = SFSettings.GetServicePath(info.ServiceDesc, _service.ServiceBasePath);
            string mainAssemblyFile = SFSettings.GetServiceMainAssemblyPath(info.ServiceDesc, _service.ServiceBasePath);
            if (!File.Exists(mainAssemblyFile))
            {
                StreamUtility.DecompressToDirectory(content, servicePath);
                ServiceDeployPackageInfo.SerializeToFile(info, SFSettings.ServiceDeployPackageInfoFile);
            }

            _wrapper.Remove();
        }

        private string _GetDeployPackageInfoFile(Guid deployPackageId)
        {
            string packagePath = SFSettings.GetServiceDeployPackagePath(_service.DeployPackagePath);
            return Path.Combine(packagePath, deployPackageId.ToString() + ".deployPackageInfo");
        }

        private string _GetDeployPackageFile(Guid deployPackageId)
        {
            string packagePath = SFSettings.GetServiceDeployPackagePath(_service.DeployPackagePath);
            return Path.Combine(packagePath, deployPackageId.ToString() + ".deployPackage");
        }

        /// <summary>
        /// 部署到所有正在运行该服务的终端
        /// </summary>
        /// <param name="deployPackageId"></param>
        public void DeployToAllClients(Guid deployPackageId)
        {
            ServiceDeployPackageInfo info = GetPackageInfo(deployPackageId, true);
            Guid[] clientIds = _service.AppClientManager.GetAllClientInfos()
                .Where(ci => ci.ServiceInfos.Any(si => si.ServiceDesc == info.ServiceDesc)).Select(si => si.ClientId).ToArray();

            _DeployToClients(info, clientIds);
        }

        /// <summary>
        /// 部署到指定的终端
        /// </summary>
        /// <param name="deployPackageId"></param>
        /// <param name="clientIds"></param>
        public void DeployToClients(Guid deployPackageId, Guid[] clientIds)
        {
            Contract.Requires(clientIds != null);
            _DeployToClients(GetPackageInfo(deployPackageId, true), clientIds);
        }

        private readonly ThreadSafeDictionaryWrapper<ServiceDesc, DeployExecutor> _deployExecutors = new ThreadSafeDictionaryWrapper<ServiceDesc, DeployExecutor>();

        // 将服务安装包部署到指定的终端
        private void _DeployToClients(ServiceDeployPackageInfo packageInfo, Guid[] clientIds)
        {
            ServiceDesc serviceDesc = packageInfo.ServiceDesc;

            string deployPackagePath = _GetDeployPackageFile(packageInfo.Id);
            if (!File.Exists(deployPackagePath))
                throw new ServiceException(ServerErrorCode.DataNotReady, "服务安装包不存在:" + packageInfo);

            try
            {
                _service.DeployTaskLocker.Lock(packageInfo.ServiceDesc, DeployLockerType.Monopoly, "部署服务：" + packageInfo.ServiceDesc.ToString());
                string servicePath = SFSettings.GetServicePath(serviceDesc, _service.ServiceBasePath);
                StreamUtility.DecompressToDirectory(File.ReadAllBytes(deployPackagePath), servicePath);
                File.Copy(_GetDeployPackageInfoFile(packageInfo.Id), Path.Combine(servicePath, SFSettings.ServiceDeployPackageInfoFile), true);

                if (!clientIds.IsNullOrEmpty())
                {
                    // 如果为TrayService，则重启这些终端
                    if (serviceDesc.IsTrayService())
                    {
                        _service.AppClientManager.RestartClients(clientIds);
                        return;
                    }

                    // 开始部署
                    DeployExecutor deployExecutor = new DeployExecutor(_service, packageInfo, clientIds);
                    _deployExecutors.Add(serviceDesc, deployExecutor);
                    deployExecutor.Execute(() => {
                        _deployExecutors.Remove(serviceDesc);
                        _service.DeployTaskLocker.Unlock(serviceDesc);
                    });
                }
            }
            catch (Exception ex)
            {
                _deployExecutors.Remove(serviceDesc);
                _service.DeployTaskLocker.Unlock(serviceDesc);
                LogManager.LogError(ex);
            }
        }

        /// <summary>
        /// 删除服务安装包
        /// </summary>
        /// <param name="deplyPackageIds"></param>
        public void Delete(Guid[] deplyPackageIds)
        {
            Contract.Requires(deplyPackageIds != null);

            if (deplyPackageIds.Length == 0)
                return;

            ServiceDeployPackageInfo[] allCurrentPackageInfos = GetAllCurrentInfos();
            ServiceDeployPackageInfo[] packageInfos = allCurrentPackageInfos
                .Where(ci => deplyPackageIds.Contains(ci.Id) && _service.AppClientManager.IsRunning(ci.ServiceDesc)).ToArray();

            if (packageInfos.Length > 0)
            {
                throw new ServiceException(ServerErrorCode.InvalidOperation, string.Format("安装包“{0}”当前正在使用，不能删除。", string.Join(",", packageInfos.Select(pi => pi.Title))));
            }

            foreach (Guid id in deplyPackageIds)
            {
                lock (_GetKeyOfDeployPackageId(id))
                {
                    PathUtility.DeleteIfExists(_GetDeployPackageFile(id));
                    PathUtility.DeleteIfExists(_GetDeployPackageInfoFile(id));

                    ServiceDeployPackageInfo curPackageInfo = allCurrentPackageInfos.FirstOrDefault(ci => ci.Id == id);
                    if (curPackageInfo != null)
                    {
                        string servicePath = SFSettings.GetServicePath(curPackageInfo.ServiceDesc, _service.ServiceBasePath);
                        PathUtility.DeleteIfExists(servicePath);
                    }
                }
            }

            _wrapper.Remove();
        }

        /// <summary>
        /// 获取服务的部署进度
        /// </summary>
        /// <param name="serviceDesces"></param>
        /// <returns></returns>
        public ServiceDeployProgress[] GetProgreses(ServiceDesc[] serviceDesces)
        {
            Contract.Requires(serviceDesces != null);

            var list = from sd in serviceDesces
                       let executor = _deployExecutors.GetOrDefault(sd)
                       where executor != null
                       select executor.GetProgress();

            return list.SelectMany().ToArray();
        }
    }
}
