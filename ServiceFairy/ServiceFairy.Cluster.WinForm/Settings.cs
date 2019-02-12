using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Configuration;
using Common.Utility;
using System.IO;
using ServiceFairy.Install;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Cluster.WinForm
{
    static class Settings
    {
        static Settings()
        {
            string configFile = Path.Combine(PathUtility.GetExecutePath(), "Configuration.xml");
            if(File.Exists(configFile))
            {
                try
                {
                    _runningConfiguration = RunningConfiguration.LoadFromFile(configFile);
                }
                catch { }
            }
        }

        private static readonly char[] _sp = new char[] { ',', ';' };

        private static string[] _Split(string s)
        {
            return (s ?? string.Empty).Split(_sp, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 默认的服务器地址
        /// </summary>
        public static CommunicationOption DefaultCommunication
        {
            get
            {
                CommunicationOption op;
                if (_runningConfiguration != null)
                {
                    op = TrayUtility.PickCommunicationOption(_Split(_runningConfiguration.MasterCommunications)
                        .SelectFromArray(s => CommunicationOption.Parse(s)));

                    if (op != null)
                        return op;
                }

                string s0 = ConfigurationManager.AppSettings.Get("defaultServerAddress");
                CommunicationOption sa;
                if (CommunicationOption.TryParse(s0, out sa))
                    return sa;

                return new CommunicationOption(new ServiceAddress("127.0.0.1", 8090));
            }
        }

        private static readonly RunningConfiguration _runningConfiguration;

        /// <summary>
        /// 运行路径
        /// </summary>
        public static string RunningPath
        {
            get
            {
                if (_runningConfiguration != null)
                    return Path.Combine(_runningConfiguration.ServiceBasePath, "Cluster\\RunningPath");

                return _GetFullPath(CommonUtility.GetFromAppConfig("runningPath", SFSettings.DefaultRunningPath));
            }
        }

        private static string _GetFullPath(string path)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }

        /// <summary>
        /// 安装包路径
        /// </summary>
        public static string DeployPackagePath
        {
            get
            {
                if (_runningConfiguration != null)
                {
                    return Path.Combine(_runningConfiguration.ServiceBasePath, "Cluster\\DeployPackage");
                }

                return _GetFullPath(CommonUtility.GetFromAppConfig("deployPackagePath", SFSettings.DefaultDeployPackagePath));
            }
        }

        /// <summary>
        /// 服务路径
        /// </summary>
        public static string ServicePath
        {
            get
            {
                if (_runningConfiguration != null)
                {
                    return Path.Combine(_runningConfiguration.ServiceBasePath, "Cluster\\Service");
                }

                return _GetFullPath(CommonUtility.GetFromAppConfig("servicePath", SFSettings.DefaultServicePath));
            }
        }
    }
}
