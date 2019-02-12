using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using Common.Package.GlobalTimer;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Package.Service;
using System.IO;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.Master;
using ServiceFairy.Entities.Deploy;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceFairy.Service.Deploy.Components
{
    /// <summary>
    /// 安装包部署管理器
    /// </summary>
    [AppComponent("安装包部署管理器", "管理服务的安装包，并提供下载")]
    class DeployPackageManager : TimerAppComponentBase
    {
        public DeployPackageManager(Service service)
            : base(service, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(1))
        {
            _service = service;
        }

        private readonly Service _service;

        protected override void OnExecuteTask(string taskName)
        {
            if (AutoRetry((Func<bool>)_Update, TimeSpan.FromSeconds(10)))
            {
                _avaliable = true;
                _waitForAvaliable.Set();
            }
        }

        #region Class PackageInfo ...

        class PackageInfo
        {
            /// <summary>
            /// 版本
            /// </summary>
            public DateTime LastUpdate { get; set; }
        }

        #endregion

        private readonly Dictionary<ServiceDesc, PackageInfo> _dict = new Dictionary<ServiceDesc, PackageInfo>();
        private readonly ManualResetEvent _waitForAvaliable = new ManualResetEvent(false);
        private bool _avaliable = false;

        /// <summary>
        /// 是否处于有效状态
        /// </summary>
        /// <returns></returns>
        public bool IsAvaliable()
        {
            return _avaliable;
        }

        /// <summary>
        /// 更新
        /// </summary>
        private bool _Update()
        {
            try
            {
                ServiceDeployPackageInfo[] infos = _service.Invoker.Master.GetAllServiceDeployPackageInfos();
                if (infos == null)
                    return false;

                bool succeed = true;
                Parallel.ForEach<ServiceDeployPackageInfo>(infos, info => {
                    try
                    {
                        PackageInfo packageInfo = _GetPackageInfo(info.ServiceDesc);
                        string path = DeployUtility.GetDeployPackagePath(_service, info.ServiceDesc);
                        if (packageInfo.LastUpdate != info.LastUpdate || !File.Exists(Path.Combine(path, _packageName)))
                        {
                            _DownloadPackage(path, info.ServiceDesc);
                            packageInfo.LastUpdate = info.LastUpdate;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                        succeed = false;
                    }
                });

                return succeed;
            }
            catch(Exception ex)
            {
                LogManager.LogError(ex);

                return false;
            }
        }

        private PackageInfo _GetPackageInfo(ServiceDesc serviceDesc)
        {
            lock (_dict)
            {
                return _dict.GetOrSet(serviceDesc);
            }
        }

        private const string _packageName = "main.gzip";

        private void _DownloadPackage(string path, ServiceDesc serviceDesc)
        {
            DeployPackage dp = _service.Invoker.Master.DownloadServiceDeployPackage(serviceDesc);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllBytes(Path.Combine(path, _packageName), dp.Content);
        }

        /// <summary>
        /// 尝试获取安装包
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="deplyPackage"></param>
        /// <returns></returns>
        public ServiceResult TryGetDeployPackage(ServiceDesc serviceDesc, out DeployPackage deplyPackage)
        {
            Contract.Requires(serviceDesc != null);

            deplyPackage = null;

#if !DEBUG
            if (!_waitForAvaliable.WaitOne(1 * 1000))
                return new ServiceResult(ServerErrorCode.DataNotReady, string.Format("服务“{0}”安装包尚未准备好", serviceDesc));
#endif

            string path = Path.Combine(DeployUtility.GetDeployPackagePath(_service, serviceDesc), _packageName);
            if (!File.Exists(path))
                return new ServiceResult(ServerErrorCode.NoData, string.Format("安装包“{0}”不存在", serviceDesc));

            deplyPackage = new DeployPackage() { Content = File.ReadAllBytes(path), Format = DeployPackageFormat.GZipCompress };
            return ServiceResult.Success;
        }

        /// <summary>
        /// 获取安装包
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public DeployPackage GetDeployPackage(ServiceDesc serviceDesc)
        {
            DeployPackage deployPackage;
            TryGetDeployPackage(serviceDesc, out deployPackage).Validate();
            return deployPackage;
        }

        protected override void OnStop()
        {
            base.OnStop();

            _waitForAvaliable.Set();
        }
    }
}
