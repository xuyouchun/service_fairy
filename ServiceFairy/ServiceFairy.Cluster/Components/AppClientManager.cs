using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.IO;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using System.Runtime.Serialization;
using Common.Package;

namespace ServiceFairy.Cluster.Components
{
    /// <summary>
    /// 服务终端管理器
    /// </summary>
    public class AppClientManager : AppComponent
    {
        public AppClientManager(ClusterContext context)
            : base(context)
        {
            _context = context;
        }

        private readonly ClusterContext _context;
        private readonly HashSet<AppClient> _clients = new HashSet<AppClient>();

        /// <summary>
        /// 添加一个服务终端
        /// </summary>
        public AppClient CreateNew()
        {
            lock (_clients)
            {
                AppClient client = _CreateNewClient();
                _clients.Add(client);

                return client;
            }
        }

        private void _DownloadClientPackage(ServiceDesc serviceDesc, Guid clientId)
        {
            /*string path = ServiceFairyUtility.GetPathOfService(serviceDesc, _context.BasePath, clientId);
            DeployPackage package = _context.SystemInvoker.Master.DownloadDeployPackage(Settings.TrayServiceDesc);
            StreamUtility.DecompressToDirectory(package.Content, path);*/
        }

        /// <summary>
        /// 删除一个服务终端
        /// </summary>
        /// <param name="client"></param>
        public void Remove(AppClient client)
        {
            Contract.Requires(client != null);

            lock (_clients)
            {
                _UnloadClient(client);

                string basePath = ServiceFairyUtility.GetAppClientRunningPath(_context.RunningPath, client.ClientID);
                if (Directory.Exists(basePath))
                    Directory.Move(basePath, basePath + "_Deleted");

                _clients.Remove(client);
            }
        }

        /// <summary>
        /// 获取所有服务终端
        /// </summary>
        /// <returns></returns>
        public AppClient[] GetAllClients()
        {
            lock (_clients)
            {
                return _clients.OrderBy(client => client.Title).ToArray();
            }
        }

        /// <summary>
        /// 获取终端数量
        /// </summary>
        /// <returns></returns>
        public int GetClientCount()
        {
            return _clients.Count;
        }

        /// <summary>
        /// 启动
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();

            _LoadAllClients();
        }

        // 加载所有服务终端
        private void _LoadAllClients()
        {
            string basePath = _context.RunningPath;
            if (!Directory.Exists(basePath))
                return;

            foreach (string dir in Directory.GetDirectories(basePath))
            {
                string dirName = Path.GetFileName(dir);
                Guid clientId;

                if (Guid.TryParse(dirName, out clientId))
                    _LoadClient(clientId);
            }
        }

        // 加载终端
        private void _LoadClient(Guid clientId)
        {
            lock (_clients)
            {
                _clients.Add(new AppClient(this, clientId, _context.ServicePath,
                    _context.RunningPath, _context.DeployPackagePath, _GetTitle(clientId), _context.MasterServiceCommunicationOption));
            }
        }

        // 重新加载程序集
        internal void Reload(Guid clientId)
        {
            string basePath = Path.Combine(_context.RunningPath, clientId.ToString());
            _DownloadClientPackage(Settings.TrayServiceDesc, clientId);
        }

        // 创建新的服务终端
        private AppClient _CreateNewClient()
        {
            Guid clientId = Guid.NewGuid();
            string basePath = ServiceFairyUtility.GetAppClientRunningPath(_context.RunningPath, clientId);
            string title;

            if (!Directory.Exists(basePath))
            {
                try
                {
                    Directory.CreateDirectory(basePath);
                    int currentIndex = ClusterCreationInfo.IncreaseCurrentIndex(_context.RunningPath);
                    title = "Cluster " + currentIndex.ToString().PadLeft(4, '0');
                    ClusterConfig clusterCfg = new ClusterConfig { Title = title };
                    File.WriteAllText(Path.Combine(basePath, "config.xml"), ClusterConfig.ToXml(clusterCfg));
                    _DownloadClientPackage(Settings.TrayServiceDesc, clientId);
                }
                catch (Exception)
                {
                    Directory.Delete(basePath, true);
                    throw;
                }
            }
            else
            {
                title = _GetTitle(clientId);
            }

            return new AppClient(this, clientId, _context.ServicePath, _context.RunningPath, _context.DeployPackagePath, title, _context.MasterServiceCommunicationOption);
        }

        private string _GetTitle(Guid clientId)
        {
            string cfgFile = Path.Combine(ServiceFairyUtility.GetAppClientRunningPath(_context.RunningPath, clientId), "config.xml");
            if (!File.Exists(cfgFile))
                return clientId.ToString();

            try
            {
                ClusterConfig cfg = ClusterConfig.FromXml(File.ReadAllText(cfgFile));
                return cfg.Title;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return clientId.ToString();
            }
        }

        private void _UnloadClient(AppClient client)
        {
            client.Stop();
        }

        [DataContract]
        class ClusterCreationInfo
        {
            [DataMember]
            public int CurrentIndex { get; set; }

            private static string _GetFilePath(string path)
            {
                return Path.Combine(path, "clusterCreationInfo.xml");
            }

            public static int ReadCurrentIndex(string path)
            {
                string file = _GetFilePath(path);
                string xml = PathUtility.ReadAllTextIfExists(file);

                if (xml == null) return 1;
                else return XmlUtility.Deserialize<ClusterCreationInfo>(xml).CurrentIndex;
            }

            public static void SaveCurrentIndex(string path, int index)
            {
                string file = _GetFilePath(path);
                string xml = XmlUtility.SerializeToString(new ClusterCreationInfo { CurrentIndex = index });
                File.WriteAllText(file, xml);
            }

            public static int IncreaseCurrentIndex(string path)
            {
                int index = ReadCurrentIndex(path);
                SaveCurrentIndex(path, index + 1);
                return index;
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            lock (_clients)
            {
                _clients.ToArray().ForEach(client => client.Dispose());
            }
        }
    }
}
