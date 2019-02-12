using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities.Master;

namespace ServiceFairy
{
    public static class SFUtility
    {
        /// <summary>
        /// 加载服务UI相关信息
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="serviceBasePath"></param>
        /// <returns></returns>
        public static ServiceUIInfo LoadServiceUIInfo(ServiceDesc serviceDesc, string serviceBasePath)
        {
            Contract.Requires(serviceDesc != null);

            string path = SFSettings.GetServicePath(serviceDesc, serviceBasePath);
            return LoadServiceUIInfo(path);
        }

        /// <summary>
        /// 加载服务UI相关信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ServiceUIInfo LoadServiceUIInfo(string path)
        {
            string assemblyFile = Path.Combine(path, SFSettings.MainAssemblyFile);
            string uiAssemblyFile = Path.Combine(path, SFSettings.UIAssemblyFile);
            string uiConfigFile = Path.Combine(path, SFSettings.UIAssemblyConfigFile);
            string iconFile = Path.Combine(path, SFSettings.MainIconFile);

            if (!File.Exists(assemblyFile))
                return null;

            AppServiceAttribute attr = LoadAppServiceAttributeFromAssembly(assemblyFile);

            return new ServiceUIInfo() {
                Title = (attr == null) ? "" : attr.Title,
                Desc = (attr == null) ? "" : attr.Desc,
                Weight = (attr == null) ? 0 : attr.Weight,
                Category = (attr == null) ? AppServiceCategory.Application : attr.Category,
                ServiceDesc = new ServiceDesc(attr.Name, attr.Version),
                AppConfig = PathUtility.ReadAllTextIfExists(uiConfigFile),
                Icon = PathUtility.ReadAllBytesIfExists(iconFile),
                MainAssembly = PathUtility.ReadAllBytesIfExists(uiAssemblyFile),
            };
        }

        /// <summary>
        /// 从指定的程序集中加载AppServiceAttribute
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        public static AppServiceAttribute LoadAppServiceAttributeFromAssembly(string assemblyFile)
        {
            Contract.Requires(assemblyFile != null);
            if (!File.Exists(assemblyFile))
                throw new FileNotFoundException("指定的程序集不存在:" + assemblyFile);

            Attribute[] attrs = AppDomainServiceLoader.GetAttributes(assemblyFile, typeof(AppServiceAttribute), true);
            return attrs.FirstOrDefault() as AppServiceAttribute;
        }
    }
}
