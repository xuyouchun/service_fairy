using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.IO;
using Common.Utility;
using Common.Contracts;

namespace ServiceFairy.Dev
{
    static class Utility
    {
        /// <summary>
        /// 获取服务的安装目录，如果不存在则返回默认值
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static string GetInstallPathOfService(ServiceDesc serviceDesc)
        {
            string installServiceBasePath = Path.Combine(PathUtility.GetExecutePath(), "Service", serviceDesc.Name);
            string path;
            if (!serviceDesc.Version.IsEmpty)
            {
                path = Path.Combine(installServiceBasePath, serviceDesc.Version.ToString());
            }
            else
            {
                SVersion[] versions = Directory.GetDirectories(installServiceBasePath)
                    .Select(path0 => _ParseToVersion(Path.GetDirectoryName(path0))).ToArray();

                if (versions.Length == 0)
                    return null;

                path = Path.Combine(installServiceBasePath, versions.Max().ToString());
            }

            if (Directory.Exists(path))
                return path;

            return null;
        }

        private static SVersion _ParseToVersion(string s)
        {
            SVersion v;
            if (SVersion.TryParse(s, out v))
                return v;

            return SVersion.Empty;
        }
    }
}
