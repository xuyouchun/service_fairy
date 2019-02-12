using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Cluster.Components;
using System.IO;
using Common.Contracts.Service;
using Common.Package;
using Common.Framework.TrayPlatform;
using Common.Utility;

namespace ServiceFairy.Cluster
{
    /// <summary>
    /// 服务终端
    /// </summary>
    public class AppClient : MarshalByRefObjectEx, IDisposable
    {
        internal AppClient(AppClientManager owner, Guid clientId, string servicePath, string runningPath, string deployPackagePath, string title, CommunicationOption serviceCo)
        {
            _owner = owner;
            ClientID = clientId;
            Title = title;
            _servicePath = servicePath;
            _runningPath = runningPath;
            _deployPackagePath = deployPackagePath;
            _clientBasePath = ServiceFairyUtility.GetAppClientRunningPath(_runningPath, clientId);
            _serverCo = serviceCo;
        }

        private readonly AppClientManager _owner;
        private readonly string _servicePath, _runningPath, _clientBasePath, _deployPackagePath;
        private readonly CommunicationOption _serverCo;
        private readonly object _thisLocker = new object();
        private bool _reloaded = false;

        /// <summary>
        /// 基路径
        /// </summary>
        public string RunningPath
        {
            get { return _runningPath; }
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid ClientID { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        private AppClientApplicationHandle _handle;

        /// <summary>
        /// 启动该服务终端
        /// </summary>
        /// <param name="packageId"></param>
        public void Start(Guid packageId = default(Guid))
        {
            lock (_thisLocker)
            {
                if (_running)
                    return;

                _Start(packageId);
                _running = true;
            }
        }

        private void _Start(Guid packageId)
        {
            string assemblyFile = ServiceFairyUtility.GetRunningMainAssemblyPath(Settings.TrayServiceDesc, _runningPath, ClientID);

            if (!_reloaded || !File.Exists(assemblyFile))
            {
                //_owner.Reload(ClientID);  // 重新加载程序集
                _reloaded = true;
            }
           
            /*if (!File.Exists(assemblyFile))
                throw new InvalidOperationException("程序集不存在:" + assemblyFile);*/

            string name = Path.GetFileNameWithoutExtension(_clientBasePath);
            AppDomainSetup setup = new AppDomainSetup() {
                ApplicationName = name,
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
            };

            if (packageId != default(Guid))
                _packageId = packageId;

            AppDomain domain = null;

            try
            {
                domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
                IAppClientApplicationLoader loader = domain.CreateInstanceAndUnwrap(typeof(IAppClientApplicationLoader).Assembly.FullName,
                    typeof(AppClientApplicationLoader).FullName) as IAppClientApplicationLoader;

                _handle = loader.Load(new AppClientApplicationLoaderContext(_servicePath, _runningPath, _deployPackagePath, ClientID, _serverCo), _Callback);
                _handle.Start(_packageId);
                _domain = domain;
            }
            catch (Exception)
            {
                if (domain != null)
                    AppDomain.Unload(domain);

                throw;
            }
        }

        private void _Restart(Guid packageId = default(Guid))
        {
            ThreadUtility.StartNew(delegate {
                try
                {
                    Stop();
                    Start(packageId);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            });
        }

        // 模拟终端的回调
        private void _Callback(string action, string[] args)
        {
            ParameterReader pr = new ParameterReader(args);
            if (action == "Exit")
            {
                _handle.Stop();

            }
            else if (action == "Restart")
            {
                _Restart();
            }
            else if (action == "LiveUpdate")
            {
                Guid pkdId;
                Guid.TryParse(pr.GetArg("/pkgId"), out pkdId);
                _Restart(pkdId);
            }
        }

        private Guid _packageId;

        private volatile bool _running;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool Running
        {
            get { return _running; }
            set
            {
                lock (_thisLocker)
                {
                    if (value)
                        Start();
                    else
                        Stop();
                }
            }
        }

        private AppDomain _domain;

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            lock (_thisLocker)
            {
                if (!_running)
                    return;

                _Stop();
                _running = false;
            }
        }

        private void _Stop()
        {
            if (_domain != null)
            {
                try
                {
                    if (_handle != null)
                        _handle.Stop(true);

                    AppDomain.Unload(_domain);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }

            _domain = null;
        }

        public void Dispose()
        {
            this.Stop();
        }
    }
}
