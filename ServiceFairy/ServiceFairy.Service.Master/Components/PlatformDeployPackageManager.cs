using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Master;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.IO;
using Common.Package;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Deploy;
using Common;
using ServiceFairy.Entities;


namespace ServiceFairy.Service.Master.Components
{
    [AppComponent("平台安装包管理器", "上传、下载及部署平台的安装包")]
    partial class PlatformDeployPackageManager : AppComponent
    {
        public PlatformDeployPackageManager(Service service)
            : base(service)
        {
            _service = service;
            _wrapper = new SingleMemoryCache<Wrapper>(TimeSpan.FromSeconds(5), _Load);
        }

        private readonly Service _service;

        private readonly SingleMemoryCache<Wrapper> _wrapper;

        private Wrapper _Load()
        {
            string deployPackagePath = SFSettings.GetPlatformDeployPackagePath(_service.DeployPackagePath);

            PlatformDeployPackageInfo[] infos = Directory.GetFiles(deployPackagePath, "*.deployPackageInfo")
                .ToArrayNotNull(file => _TryLoadDeployPackageInfo(file));

            return new Wrapper() {
                Infos = infos,
                Dict = infos.ToDictionary(info => info.Id, true),
            };
        }

        class Wrapper
        {
            public PlatformDeployPackageInfo[] Infos;
            public Dictionary<Guid, PlatformDeployPackageInfo> Dict;
        }

        /// <summary>
        /// 获取所有平台安装包的信息
        /// </summary>
        /// <returns></returns>
        public PlatformDeployPackageInfo[] GetAllInfos()
        {
            return _wrapper.Get().Infos;
        }

        private PlatformDeployPackageInfo _TryLoadDeployPackageInfo(string file)
        {
            try
            {
                Guid id;
                if (!File.Exists(file) || !Guid.TryParse(Path.GetFileNameWithoutExtension(file), out id))
                    return null;

                lock (_GetKeyOfDeployPackageId(id))
                {
                    return PlatformDeployPackageInfo.DeserailizeFromFile(file);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return null;
            }
        }

        private object _GetKeyOfDeployPackageId(Guid deployPackageId)
        {
            return string.Intern(deployPackageId.ToString().ToLower());
        }

        /// <summary>
        /// 获取指定ID的安装包信息
        /// </summary>
        /// <param name="deployPackageId"></param>
        /// <returns></returns>
        public PlatformDeployPackageInfo GetInfo(Guid deployPackageId)
        {
            return _wrapper.Get().Dict.GetOrDefault(deployPackageId);
        }

        /// <summary>
        /// 上传安装包
        /// </summary>
        /// <param name="info"></param>
        /// <param name="content"></param>
        public void Upload(PlatformDeployPackageInfo info, byte[] content)
        {
            Contract.Requires(info != null && content != null);

            string packagePath = SFSettings.GetPlatformDeployPackagePath(_service.DeployPackagePath);

            if (info.Id == Guid.Empty)
                info.Id = Guid.NewGuid();

            string path = Path.Combine(packagePath, info.Id.ToString());
            lock (_GetKeyOfDeployPackageId(info.Id))
            {
                PlatformDeployPackageInfo.SerializeToFile(info, path + ".deployPackageInfo");
                File.WriteAllBytes(path + ".deployPackage", content);
            }

            _wrapper.Remove();
        }

        /// <summary>
        /// 下载安装包
        /// </summary>
        /// <param name="deployPackageId"></param>
        /// <returns></returns>
        public DeployPackage Download(Guid deployPackageId)
        {
            string path = Path.Combine(SFSettings.GetPlatformDeployPackagePath(_service.DeployPackagePath), deployPackageId.ToString());

            string deployPackageInfoPath = path + ".deployPackageInfo";
            string deployPackagePath = path + ".deployPackage";

            if (!File.Exists(deployPackageInfoPath) || !File.Exists(deployPackagePath))
                return null;

            return new DeployPackage() {
                Content = File.ReadAllBytes(deployPackagePath),
                Format = DeployPackageFormat.GZipCompress,
            };
        }

        /// <summary>
        /// 删除安装包
        /// </summary>
        /// <param name="deployPackageIds"></param>
        public void Delete(Guid[] deployPackageIds)
        {
            Contract.Requires(deployPackageIds != null);

            string deployPackagePath = SFSettings.GetPlatformDeployPackagePath(_service.DeployPackagePath);
            foreach (Guid id in deployPackageIds)
            {
                string path = Path.Combine(deployPackagePath, id.ToString());
                lock (_GetKeyOfDeployPackageId(id))
                {
                    PathUtility.DeleteIfExists(path + ".deployPackageInfo");
                    PathUtility.DeleteIfExists(path + ".deployPackage");
                }
            }

            _wrapper.Remove();
        }

        /// <summary>
        /// 部署到所有终端
        /// </summary>
        /// <param name="deployPackageId"></param>
        public void DeployToAllClients(Guid deployPackageId)
        {
            DeployToClients(deployPackageId, _service.AppClientManager.GetAllClientDescs().ToArray(client => client.ClientID));
        }

        /// <summary>
        /// 部署到指定的终端
        /// </summary>
        /// <param name="deployPackageId"></param>
        /// <param name="clientIds"></param>
        public void DeployToClients(Guid deployPackageId, Guid[] clientIds)
        {
            Contract.Requires(clientIds != null);
            if (clientIds.Length == 0)
                return;

            DeployPackageInfo deployPackageInfo = GetInfo(deployPackageId);
            if (deployPackageInfo == null)
                throw new ServiceException(ServerErrorCode.NoData, "该平台安装包不存在");
            
            _service.DeployTaskLocker.Lock(_service, DeployLockerType.GlobalMonopoly, "平台部署，版本：" + deployPackageId);
            _deployExecutor = new DeployExecutor(_service, deployPackageInfo, clientIds);
            _deployExecutor.Execute(() => _service.DeployTaskLocker.Unlock(_service));
        }

        private volatile DeployExecutor _deployExecutor;

        /// <summary>
        /// 获取部署进度
        /// </summary>
        /// <returns></returns>
        public PlatformDeployProgress[] GetProgress()
        {
            DeployExecutor executor = _deployExecutor;

            if (executor == null)
                return Array<PlatformDeployProgress>.Empty;

            return executor.GetProgress();
        }
    }
}
