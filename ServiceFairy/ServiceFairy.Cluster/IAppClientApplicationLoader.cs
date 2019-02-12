using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Threading;
using Common.Package;
using Common.Utility;
using System.IO;
using Common.Framework.TrayPlatform;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Cluster
{
    interface IAppClientApplicationLoader
    {
        AppClientApplicationHandle Load(AppClientApplicationLoaderContext context, Action<string, string[]> callback);
    }

    [Serializable]
    class AppClientApplicationLoaderContext
    {
        public AppClientApplicationLoaderContext(string servicePath, string runningPath, string deployPackagePath, Guid clientId, CommunicationOption serverCo)
        {
            ServicePath = servicePath;
            RunningPath = runningPath;
            DeployPackagePath = deployPackagePath;
            ClientId = clientId;
            ServiceCommunicationOption = serverCo;
        }

        public string ServicePath { get; private set; }

        public string RunningPath { get; private set; }

        public string DeployPackagePath { get; private set; }

        public Guid ClientId { get; private set; }

        public CommunicationOption ServiceCommunicationOption { get; private set; }
    }

    class AppClientApplicationLoader : MarshalByRefObject, IAppClientApplicationLoader
    {
        public AppClientApplicationHandle Load(AppClientApplicationLoaderContext context, Action<string, string[]> callback)
        {
            return new AppClientApplicationHandle(context, callback);
        }
    }

    class AppClientApplicationHandle : MarshalByRefObjectEx, IDisposable
    {
        public AppClientApplicationHandle(AppClientApplicationLoaderContext context, Action<string, string[]> callback)
        {
            _context = context;
            _callback = callback;
        }

        private readonly AppClientApplicationLoaderContext _context;
        private readonly ManualResetEvent _waitForExit = new ManualResetEvent(false);
        private readonly ManualResetEvent _waitForDispose = new ManualResetEvent(false);
        private volatile bool _running;
        private readonly object _thisLocker = new object();
        private readonly Action<string, string[]> _callback;

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="deployPackageId"></param>
        public void Start(Guid deployPackageId = default(Guid))
        {
            lock (_thisLocker)
            {
                if (!_running)
                {
                    _running = true;
                    _Start(deployPackageId);
                }
            }
        }

        private void _Start(Guid deployPackageId)
        {
            AppClientInitInfo initInfo = _GetInitInfo(deployPackageId);
            ClusterServiceFairyApplication app = new ClusterServiceFairyApplication(initInfo, _context.ServiceCommunicationOption, _waitForExit);

            Thread t = new Thread(delegate() {
                try
                {
                    _waitForDispose.Reset();
                    _waitForExit.Reset();

                    app.Execute(new string[0], _callback, _waitForExit);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
                finally
                {
                    _waitForDispose.Set();
                }
            });

            t.IsBackground = true;
            t.Start();
        }

        private AppClientInitInfo _GetInitInfo(Guid deployPackageId)
        {
            string cfgXml = PathUtility.ReadAllTextIfExists(
                Path.Combine(ServiceFairyUtility.GetAppClientRunningPath(_context.RunningPath, _context.ClientId), "config.xml"));

            ClusterConfig cfg;
            if (!string.IsNullOrEmpty(cfgXml))
                cfg = ClusterConfig.FromXml(cfgXml);
            else
                cfg = new ClusterConfig() { Title = "" };

            return new AppClientInitInfo(_context.ClientId, deployPackageId, _context.RunningPath, _context.ServicePath, _context.DeployPackagePath,
                cfg.Title + " on " + NetworkUtility.GetHostName(),
                string.Format("使用集群模拟器", NetworkUtility.GetHostName())
            );
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="waitForExit"></param>
        public void Stop(bool waitForExit = false)
        {
            lock (_thisLocker)
            {
                if (_running)
                {
                    _waitForExit.Set();

                    if (waitForExit)
                        _waitForDispose.WaitOne();

                    _running = false;
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }
}
