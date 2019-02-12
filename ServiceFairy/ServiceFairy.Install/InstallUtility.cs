using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Utility;

namespace ServiceFairy.Install
{
    public static class InstallUtility
    {
        /// <summary>
        /// 从xml文件中读取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        internal static T ReadFromFile<T>(string file) where T : class
        {
            Contract.Requires(file != null);

            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(T));
                return ser.ReadObject(fs) as T;
            }
        }

        /// <summary>
        /// 将实体保存到xml文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="obj"></param>
        internal static void SaveToFile(string file, object obj)
        {
            Contract.Requires(file != null && obj != null);

            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                DataContractSerializer ser = new DataContractSerializer(obj.GetType());
                ser.WriteObject(fs, obj);
            }
        }

        /// <summary>
        /// 获取服务的安装目录，如果不存在则返回默认值
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static string GetInstallPathOfService(ref ServiceDesc serviceDesc)
        {
            string installServiceBasePath = Path.Combine(PathUtility.GetExecutePath(), "Service", serviceDesc.Name);
            if (!Directory.Exists(installServiceBasePath))
                return null;

            string path;
            SVersion version = serviceDesc.Version;
            if (!serviceDesc.Version.IsEmpty)
            {
                path = Path.Combine(installServiceBasePath, serviceDesc.Version.ToString());
            }
            else
            {
                SVersion[] versions = Directory.GetDirectories(installServiceBasePath)
                    .Select(path0 => _ParseToVersion(Path.GetFileName(path0))).Where(ver => !ver.IsEmpty).ToArray();

                if (versions.Length == 0)
                    return null;

                version = versions.Max();
                path = Path.Combine(installServiceBasePath, version.ToString());
            }

            if (Directory.Exists(path))
            {
                serviceDesc = new ServiceDesc(serviceDesc.Name, version);
                return path;
            }

            return null;
        }

        private static SVersion _ParseToVersion(string s)
        {
            SVersion v;
            if (SVersion.TryParse(s, out v))
                return v;

            return SVersion.Empty;
        }

        /// <summary>
        /// 获取安装包的部署基路径
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static string GetDeployPackageBasePath(string serviceBasePath)
        {
            return Path.Combine(serviceBasePath, "DeployPackage");
        }

        /// <summary>
        /// 获取数据目录
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static string GetDataPath(string serviceBasePath)
        {
            return Path.Combine(serviceBasePath, "Data");
        }

        /// <summary>
        /// 获取日志目录
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static string GetLogPath(string serviceBasePath)
        {
            return Path.Combine(serviceBasePath, "Log");
        }

        /// <summary>
        /// 获取安装包的部署路径
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static string GetServicePath(string serviceBasePath, ServiceDesc serviceDesc)
        {
            if (serviceDesc.Version.IsEmpty)
                throw new ArgumentException("版本号不可以为空");

            return SFSettings.GetServicePath(serviceDesc, GetServiceBasePath(serviceBasePath));
        }

        /// <summary>
        /// 获取运行路径
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static string GetRunningPath(string serviceBasePath)
        {
            return Path.Combine(serviceBasePath, "RunningService");
        }

        /// <summary>
        /// 获取服务路径
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static string GetServiceBasePath(string serviceBasePath)
        {
            return Path.Combine(serviceBasePath, "Service");
        }

        /// <summary>
        /// 获取服务的配置文件路径
        /// </summary>
        /// <param name="serviceBasePath"></param>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static string GetConfigPath(string serviceBasePath, ServiceDesc serviceDesc)
        {
            if (serviceDesc.Version.IsEmpty)
                throw new ArgumentException("版本号不可以为空");

            return SFSettings.GetServiceConfigPath(serviceDesc, GetDeployPackageBasePath(serviceBasePath));
        }

        /// <summary>
        /// 从安装目录中加载所有的服务
        /// </summary>
        /// <returns></returns>
        public static ServiceDesc[] GetAllServiceDescsFromInstallPath()
        {
            string servicePath = Path.Combine(PathUtility.GetExecutePath(), "Service");
            List<ServiceDesc> sds = new List<ServiceDesc>();

            foreach (string serviceInstallPath in Directory.GetDirectories(servicePath))
            {
                string serviceName = Path.GetFileName(serviceInstallPath);
                foreach (string versionPath in Directory.GetDirectories(serviceInstallPath))
                {
                    string version = Path.GetFileName(versionPath);
                    SVersion sv;
                    if (SVersion.TryParse(version, out sv))
                        sds.Add(new ServiceDesc(serviceName, sv));
                }
            }

            return sds.ToArray();
        }
    }
}
