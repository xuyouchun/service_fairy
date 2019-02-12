using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Contracts;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace ServiceFairy
{
    public static class SFSettings
    {
        /// <summary>
        /// 主程序集文件的名称
        /// </summary>
        public const string MainAssemblyFile = "Main.dll";

        /// <summary>
        /// 管理界面程序集文件的名称
        /// </summary>
        public const string UIAssemblyFile = "Main.UI.dll";

        /// <summary>
        /// 服务的图标名称
        /// </summary>
        public const string MainIconFile = "Main.ico";

        /// <summary>
        /// 主程序集配置文件名称
        /// </summary>
        public const string MainAssemblyConfigFile = MainAssemblyFile + ".config";

        /// <summary>
        /// 管理界面程序集的配置文件名称
        /// </summary>
        public const string UIAssemblyConfigFile = UIAssemblyFile + ".config";

        private static readonly string[] _allCoreFiles = new[] {
            MainAssemblyFile, UIAssemblyFile, MainIconFile, MainAssemblyConfigFile, UIAssemblyConfigFile,
        };

        /// <summary>
        /// Platform安装包路径的名称
        /// </summary>
        public const string PlatformDeployPackageDirectory = "Platform";

        /// <summary>
        /// Service安装包路径的名称
        /// </summary>
        public const string ServiceDeployPackageDirectory = "Service";

        /// <summary>
        /// LiveUpdate安装包路径的名称
        /// </summary>
        public const string PlatformLiveUpdateDirectory = "LiveUpdate";

        /// <summary>
        /// 平台安装包信息文件名称
        /// </summary>
        public const string PlatformDeployPackageInfoFile = "platform.deployPackageInfo";

        /// <summary>
        /// 服务安装包信息文件名称
        /// </summary>
        public const string ServiceDeployPackageInfoFile = "service.deployPackageInfo";

        /// <summary>
        /// 平台的日志源
        /// </summary>
        public const string PlatformLogSource = "Platform";

        /// <summary>
        /// 获取所有重要的文件
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllCoreFiles()
        {
            return _allCoreFiles;
        }

        private static string _appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ServiceFairy");

        /// <summary>
        /// 默认的运行路径
        /// </summary>
        public static string DefaultRunningPath = Path.Combine(_appDataPath, "RunningService");

        /// <summary>
        /// 默认的安装包路径
        /// </summary>
        public static string DefaultDeployPackagePath = Path.Combine(_appDataPath, "DeployPackage");

        /// <summary>
        /// 默认的服务路径
        /// </summary>
        public static string DefaultServicePath = Path.Combine(_appDataPath, "Service");

        /// <summary>
        /// 默认的数据路径
        /// </summary>
        public static string DefaultDataPath = Path.Combine(_appDataPath, "Data");

        /// <summary>
        /// 默认的日志路径
        /// </summary>
        public static string DefaultLogPath = Path.Combine(_appDataPath, "Log");

        /// <summary>
        /// 获取服务的运行路径
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="runningBasePath"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static string GetServiceRunningBasePath(ServiceDesc serviceDesc, string runningBasePath, Guid clientId)
        {
            string path = Path.Combine(GetAppClientRunningPath(runningBasePath, clientId),
                string.Format(@"{0}\{1}", serviceDesc.Name, serviceDesc.Version.ToString()));
            string fullPath = Path.Combine(_GetFullPath(runningBasePath), path);
            PathUtility.CreateDirectoryIfNotExists(fullPath);

            return fullPath;
        }

        /// <summary>
        /// 获取服务终端的运行路径
        /// </summary>
        /// <param name="runningBasePath"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static string GetAppClientRunningPath(string runningBasePath, Guid clientId)
        {
            return Path.Combine(_GetFullPath(runningBasePath), clientId.ToString());
        }

        /// <summary>
        /// 获取指定服务的程序集路径，从运行路径中寻找最新的一个程序集路径
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="runningBasePath"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static string GetServiceRunningPath(ServiceDesc serviceDesc, string runningBasePath, Guid clientId)
        {
            Contract.Requires(serviceDesc != null);

            string serviceBasePath = GetServiceRunningBasePath(serviceDesc, runningBasePath, clientId);
            string[] directories = Directory.GetDirectories(serviceBasePath);
            string fullPath;
            if (directories.Length == 0)
            {
                string subPath = PathUtility.GetUtcTimePath();
                fullPath = Path.Combine(serviceBasePath, subPath);
                PathUtility.CreateDirectoryIfNotExists(fullPath);
            }
            else
            {
                fullPath = directories.Max();
            }

            return PathUtility.Revise(fullPath);
        }

        /// <summary>
        /// 清空指定服务所属路径的所有文件
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="runningBasePath"></param>
        /// <param name="clientId"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static void ClearRunningPath(ServiceDesc serviceDesc, string runningBasePath, Guid clientId, bool throwError = false)
        {
            Contract.Requires(serviceDesc != null);

            string serviceBasePath = GetServiceRunningBasePath(serviceDesc, runningBasePath, clientId);
            PathUtility.ClearPath(serviceBasePath, false, throwError);
        }

        /// <summary>
        /// 获取指定服务的程序集主文件
        /// </summary>
        /// <param name="serviceDesc">服务信息</param>
        /// <param name="runningBasePath">基路径</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns></returns>
        public static string GetRunningMainAssemblyPath(ServiceDesc serviceDesc, string runningBasePath, Guid clientId)
        {
            return Path.Combine(GetServiceRunningPath(serviceDesc, runningBasePath, clientId), SFSettings.MainAssemblyFile);
        }

        private static string _GetFullPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }

        /// <summary>
        /// 获取配置文件的内容
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="runningBasePath"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static string ReadConfigurationOfService(ServiceDesc serviceDesc, string runningBasePath)
        {
            string path = GetServiceConfigPath(serviceDesc, runningBasePath);
            if (File.Exists(path))
                return File.ReadAllText(path);

            return null;
        }

        /// <summary>
        /// 获取配置文件的内容和信息
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static AppServiceConfiguration GetServiceConfigContent(ServiceDesc serviceDesc, string serviceBasePath)
        {
            string file = GetServiceConfigPath(serviceDesc, serviceBasePath);
            if (!File.Exists(file))
                return null;

            return new AppServiceConfiguration() { Content = File.ReadAllText(file), LastUpdate = File.GetLastWriteTimeUtc(file) };
        }

        /// <summary>
        /// 获取服务的路径
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static string GetServicePath(ServiceDesc serviceDesc, string serviceBasePath)
        {
            Contract.Requires(serviceDesc != null && serviceBasePath != null);

            string path = string.Format(@"{0}\{1}", serviceDesc.Name, serviceDesc.Version.ToString());
            string fullPath = Path.Combine(_GetFullPath(serviceBasePath), path);
            return PathUtility.Revise(fullPath);
        }

        /// <summary>
        /// 获取服务的主程序集路径
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static string GetServiceMainAssemblyPath(ServiceDesc serviceDesc, string serviceBasePath)
        {
            return Path.Combine(GetServicePath(serviceDesc, serviceBasePath), SFSettings.MainAssemblyFile);
        }

        /// <summary>
        /// 获取安装包中配置文件的路径
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static string GetServiceConfigPath(ServiceDesc serviceDesc, string serviceBasePath)
        {
            return GetServicePath(serviceDesc, serviceBasePath) + ".config";
        }

        /// <summary>
        /// 获取全部配置文件
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static Dictionary<ServiceDesc, AppServiceConfiguration> GetAllConfigurations(string serviceBasePath)
        {
            Dictionary<ServiceDesc, AppServiceConfiguration> dict = new Dictionary<ServiceDesc, AppServiceConfiguration>();

            foreach (string serviceName in GetAllServiceNames(serviceBasePath))
            {
                foreach (SVersion ver in GetAllVersions(serviceName, serviceBasePath))
                {
                    ServiceDesc sd = new ServiceDesc(serviceName, ver);
                    AppServiceConfiguration cfg = GetServiceConfigContent(sd, serviceBasePath);
                    if (cfg != null)
                        dict.Add(sd, cfg);
                }
            }

            return dict;
        }

        /// <summary>
        /// 获取Platform安装包路径
        /// </summary>
        /// <param name="packagePath"></param>
        /// <returns></returns>
        public static string GetPlatformDeployPackagePath(string packagePath)
        {
            Contract.Requires(packagePath != null);

            string path = Path.Combine(packagePath, SFSettings.PlatformDeployPackageDirectory);
            path = _GetFullPath(path);
            PathUtility.CreateDirectoryIfNotExists(path);

            return path;
        }

        /// <summary>
        /// 获取服务安装包路径
        /// </summary>
        /// <param name="packagePath"></param>
        /// <returns></returns>
        public static string GetServiceDeployPackagePath(string packagePath)
        {
            Contract.Requires(packagePath != null);

            string path = Path.Combine(packagePath, SFSettings.ServiceDeployPackageDirectory);
            path = _GetFullPath(path);
            PathUtility.CreateDirectoryIfNotExists(path);

            return path;
        }

        /// <summary>
        /// 获取所有服务的名称
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static string[] GetAllServiceNames(string serviceBasePath)
        {
            return Directory.GetDirectories(_GetFullPath(serviceBasePath)).ToArray(path => Path.GetFileName(path));
        }

        /// <summary>
        /// 获取所有服务的描述
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static ServiceDesc[] GetAllServiceDescs(string serviceBasePath)
        {
            return GetAllServiceNames(serviceBasePath).SelectMany(serviceName => GetAllVersions(serviceName, serviceBasePath)
                .Select(ver => new ServiceDesc(serviceName, ver))).ToArray();
        }

        /// <summary>
        /// 获取指定服务的所有版本
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static SVersion[] GetAllVersions(string serviceName, string serviceBasePath)
        {
            Contract.Requires(serviceName != null);

            string path = Path.Combine(_GetFullPath(serviceBasePath), PathUtility.ReplaceInvalidFileNameChars(serviceName, "_"));
            if (!Directory.Exists(path))
                return new SVersion[0];

            List<SVersion> vers = new List<SVersion>();
            foreach (string dir in Directory.GetDirectories(path))
            {
                if (!File.Exists(Path.Combine(dir, SFSettings.MainAssemblyFile)))
                    continue;

                string dirName = Path.GetFileName(dir);
                SVersion ver;
                if (!SVersion.TryParse(dirName, out ver))
                    continue;

                vers.Add(ver);
            }

            return vers.ToArray();
        }

        /// <summary>
        /// 获取指定服务的最新版本
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static SVersion GetNewestVersion(string serviceName, string serviceBasePath)
        {
            SVersion[] vers = GetAllVersions(serviceName, serviceBasePath);
            if (vers.IsNullOrEmpty())
                return SVersion.Empty;

            return vers.Max();
        }
    }
}
