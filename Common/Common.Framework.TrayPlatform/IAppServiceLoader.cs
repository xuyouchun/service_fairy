using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// AppService加载器
    /// </summary>
    public interface IAppServiceLoader
    {
        /// <summary>
        /// 从指定的程序集中加载Service
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        IAppService LoadService(string assemblyFile);
    }
}
