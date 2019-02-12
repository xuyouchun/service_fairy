using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 从另一个应用程序域加载所需的服务
    /// </summary>
    public interface IAppDomainServiceLoader
    {
        /// <summary>
        /// 加载服务
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        object LoadService(string assemblyFile);

        /// <summary>
        /// 从程序集的字节流中加载服务
        /// </summary>
        /// <param name="assemblyBytes"></param>
        /// <returns></returns>
        object LoadService(byte[] assemblyBytes);

        /// <summary>
        /// 获取服务的类型
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        Type GetServiceType(string assemblyFile);

        /// <summary>
        /// 获取服务的类型
        /// </summary>
        /// <param name="assemblyBytes"></param>
        /// <returns></returns>
        Type GetServiceType(byte[] assemblyBytes);

        /// <summary>
        /// 获取入口服务类型的所有Attribute
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        Attribute[] GetServiceTypeAttributes(string assemblyFile, Type attrType, bool inherit);

        /// <summary>
        /// 获取入口服务类型的所有Attribute
        /// </summary>
        /// <param name="assemblyBytes"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        Attribute[] GetServiceTypeAttributes(byte[] assemblyBytes, Type attrType, bool inherit);
    }
}
