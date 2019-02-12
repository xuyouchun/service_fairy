using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.IO;
using Common.Contracts.Service;
using Common.Utility;

namespace ServiceFairy.Service.Deploy
{
    static class DeployUtility
    {
        /// <summary>
        /// 获取安装包根路径
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static string GetDeployPackageBasePath(Service service)
        {
            return Path.Combine(service.ServiceDataPath, "DeployPackages");
        }

        /// <summary>
        /// 获取指定服务安装包的路径
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static string GetDeployPackagePath(Service service, ServiceDesc serviceDesc)
        {
            string basePath = GetDeployPackageBasePath(service);
            return Path.Combine(basePath, PathUtility.ReplaceInvalidFileNameChars(serviceDesc.Name, "_"),
                PathUtility.ReplaceInvalidFileNameChars(serviceDesc.Version.ToString(), "_"));
        }
    }
}
