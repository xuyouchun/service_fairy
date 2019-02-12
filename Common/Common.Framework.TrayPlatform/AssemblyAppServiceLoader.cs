using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Reflection;
using Common.Package;
using System.IO;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 用于加载程序集并执行
    /// </summary>
    class AssemblyAppServiceLoader : MarshalByRefObject, IAppServiceLoader
    {
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="context">参数</param>
        /// <returns></returns>
        public IAppService LoadService(string assemblyFile)
        {
            IAppService svr = PackageUtility.GetAssemblyEntryPoint(assemblyFile) as IAppService;
            if (svr == null)
                throw new InvalidProgramException("未从程序集中找到入口点");

            return svr;
        }
    }
}
