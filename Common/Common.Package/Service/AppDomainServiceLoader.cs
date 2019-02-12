using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Utility;

namespace Common.Package.Service
{
    public class AppDomainServiceLoader : MarshalByRefObjectEx, IAppDomainServiceLoader
    {
        public object LoadService(string assemblyFile)
        {
            Contract.Requires(assemblyFile != null);

            return PackageUtility.GetAssemblyEntryPoint(assemblyFile);
        }

        public object LoadService(byte[] assemblyBytes)
        {
            Contract.Requires(assemblyBytes != null);

            return PackageUtility.GetAssemblyEntryPoint(assemblyBytes);
        }

        public Type GetServiceType(string assemblyFile)
        {
            Contract.Requires(assemblyFile != null);

            return PackageUtility.GetAssemblyEntryPointType(assemblyFile);
        }

        public Type GetServiceType(byte[] assemblyBytes)
        {
            Contract.Requires(assemblyBytes != null);

            return PackageUtility.GetAssemblyEntryPointType(assemblyBytes);
        }

        public Attribute[] GetServiceTypeAttributes(string assemblyFile, Type attributeType, bool inherit)
        {
            Type type = GetServiceType(assemblyFile);
            return _GetAttributes(type, attributeType, inherit);
        }

        public Attribute[] GetServiceTypeAttributes(byte[] assemblyBytes, Type attributeType, bool inherit)
        {
            Type type = GetServiceType(assemblyBytes);
            return _GetAttributes(type, attributeType, inherit);
        }

        private Attribute[] _GetAttributes(Type type, Type attributeType, bool inherit)
        {
            if (attributeType != null)
                return type.GetCustomAttributes(attributeType, inherit).OfType<Attribute>().ToArray();

            return type.GetCustomAttributes(inherit).OfType<Attribute>().ToArray();
        }

        public static object Load(string assemblyFile, out AppDomain domain)
        {
            Contract.Requires(assemblyFile != null);

            domain = _CreateAppDomainFromFile(assemblyFile);
            return _Load(domain, delegate(IAppDomainServiceLoader loader) {
                return loader.LoadService(assemblyFile);
            });
        }

        private static AppDomain _CreateAppDomainFromFile(string assemblyFile)
        {
            AppDomainSetup adSetup = new AppDomainSetup();
            string appName = Path.GetFileNameWithoutExtension(assemblyFile);
            adSetup.ApplicationName = appName;
            adSetup.ApplicationBase = Path.GetDirectoryName(assemblyFile);
            adSetup.ConfigurationFile = assemblyFile + ".config";    // (string)AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE");
            adSetup.ShadowCopyFiles = "true";
            return AppDomain.CreateDomain(appName, AppDomain.CurrentDomain.Evidence, adSetup);
        }

        private static object _Load(AppDomain domain, Func<IAppDomainServiceLoader, object> serviceLoader)
        {
            try
            {
                IAppDomainServiceLoader loader = domain.CreateInstanceAndUnwrap(typeof(AppDomainServiceLoader).Assembly.FullName,
                        typeof(AppDomainServiceLoader).FullName) as IAppDomainServiceLoader;

                return serviceLoader(loader);
            }
            catch (Exception)
            {
                if (domain != null)
                    domain.SafeUnload();

                throw;
            }
        }

        private static int _domainIndex = 1;

        public static object Load(byte[] assemblyBytes, string appConfig, out AppDomain domain)
        {
            Contract.Requires(assemblyBytes != null);

            domain = _CreateAppDomain(appConfig);
            return _Load(domain, delegate(IAppDomainServiceLoader loader) {
                return loader.LoadService(assemblyBytes);
            });
        }

        private static AppDomain _CreateAppDomain(string appConfig = "")
        {
            AppDomainSetup adSetup = new AppDomainSetup();
            string appName = "appdomain_" + _domainIndex;
            adSetup.ApplicationName = appName;
            adSetup.SetConfigurationBytes(Encoding.UTF8.GetBytes(appConfig ?? ""));
            return AppDomain.CreateDomain(appName, AppDomain.CurrentDomain.Evidence, adSetup);
        }

        /// <summary>
        /// 获取程序集的入口点
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="assemblyFile"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static TService Load<TService>(string assemblyFile, out AppDomain domain) where TService : class
        {
            return (TService)Load(assemblyFile, out domain);
        }

        /// <summary>
        /// 获取程序集的入口点
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="assemblyBytes"></param>
        /// <param name="appConfig"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static TService Load<TService>(byte[] assemblyBytes, string appConfig, out AppDomain domain) where TService : class
        {
            return (TService)Load(assemblyBytes, appConfig, out domain);
        }

        /// <summary>
        /// 获取程序集的入口点类型
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static Type GetEntryPointType(string assemblyFile, out AppDomain domain)
        {
            Contract.Requires(assemblyFile != null);

            domain = _CreateAppDomainFromFile(assemblyFile);
            return _Load(domain, delegate(IAppDomainServiceLoader loader) {
                return loader.GetServiceType(assemblyFile);
            }) as Type;
        }

        /// <summary>
        /// 获取程序集的入口点类型
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="appConfig"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static Type GetEntryPointType(byte[] assemblyBytes, out AppDomain domain)
        {
            Contract.Requires(assemblyBytes != null);

            domain = _CreateAppDomain();
            return _Load(domain, delegate(IAppDomainServiceLoader loader) {
                return loader.GetServiceType(assemblyBytes);
            }) as Type;
        }

        /// <summary>
        /// 获取程序集入口点类型的Attribute
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static Attribute[] GetAttributes(string assemblyFile, Type attrType = null, bool inherit = false)
        {
            Contract.Requires(assemblyFile != null);

            AppDomain domain = null;

            try
            {
                domain = _CreateAppDomainFromFile(assemblyFile);
                return _Load(domain, delegate(IAppDomainServiceLoader loader) {
                    return loader.GetServiceTypeAttributes(assemblyFile, attrType, inherit);
                }) as Attribute[];
            }
            finally
            {
                domain.SafeUnload();
            }
        }

        /// <summary>
        /// 获取程序集入口点类型的Attribute
        /// </summary>
        /// <param name="assemblyBytes"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static Attribute[] GetAttributes(byte[] assemblyBytes, Type attrType = null, bool inherit = false)
        {
            Contract.Requires(assemblyBytes != null);

            AppDomain domain = null;

            try
            {
                domain = _CreateAppDomain();
                return _Load(domain, delegate(IAppDomainServiceLoader loader) {
                    return loader.GetServiceTypeAttributes(assemblyBytes, attrType, inherit);
                }) as Attribute[];
            }
            finally
            {
                domain.SafeUnload();
            }
        }
    }
}
