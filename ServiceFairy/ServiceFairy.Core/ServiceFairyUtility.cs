using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Common;
using Common.Communication.Wcf;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using Common.Package;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities;
using AppCommandFunc = System.Func<Common.Contracts.Service.AppCommandExecuteContext, Common.Contracts.Service.InputAppCommandArg, Common.Contracts.Service.OutputAppCommandArg>;
using AppCommandFuncNoInputOutput = System.Action<Common.Contracts.Service.AppCommandExecuteContext>;
using AppCommandFuncWithoutInput = System.Func<Common.Contracts.Service.AppCommandExecuteContext, Common.Contracts.Service.OutputAppCommandArg>;
using AppCommandFuncWithoutOutput = System.Action<Common.Contracts.Service.AppCommandExecuteContext, Common.Contracts.Service.InputAppCommandArg>;

namespace ServiceFairy
{
    public static class ServiceFairyUtility
    {
        /// <summary>
        /// 是否包含指定的监听器
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool ContainsListener(this ITrayPlatform platform, ServiceAddress address)
        {
            Contract.Requires(platform != null && address != null);

            return platform.GetAllCommunicationOptions().Any(v => v.Address == address);
        }

        /// <summary>
        /// 重启监听
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="option"></param>
        /// <param name="force"></param>
        public static void RestartListener(this ITrayPlatform platform, CommunicationOption option, bool force = false)
        {
            Contract.Requires(platform != null && option != null);
            CommunicationOption old = GetCommunicationOption(platform, option.Address);

            if (old != null)
            {
                if (!force && old == option)
                    return;

                platform.StopListener(option.Address);
            }
            else
            {
                platform.StartListener(option);
            }
        }

        /// <summary>
        /// 重启监听
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="options"></param>
        /// <param name="force"></param>
        public static void RestartListeners(this ITrayPlatform platform, CommunicationOption[] options, bool force = false)
        {
            Contract.Requires(platform != null && options != null);

            foreach (CommunicationOption option in options)
            {
                RestartListener(platform, option, force);
            }
        }

        /// <summary>
        /// 获取指定的监听器
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static CommunicationOption GetCommunicationOption(this ITrayPlatform platform, ServiceAddress address)
        {
            Contract.Requires(platform != null && address != null);

            return platform.GetAllCommunicationOptions().FirstOrDefault(v => v.Address == address);
        }

        /// <summary>
        /// 获取指定名称与版本号的服务
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="serviceName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static ServiceInfo GetServiceInfo(this ITrayPlatform platform, string serviceName, SVersion version)
        {
            return platform.GetAllServiceInfos().FirstOrDefault(v => v.ServiceDesc.Name == serviceName && v.ServiceDesc.Version == version);
        }

        /// <summary>
        /// 获取指定名称与版本号的服务
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static ServiceInfo GetServiceInfo(this ITrayPlatform platform, ServiceDesc serviceDesc)
        {
            return GetServiceInfo(platform, serviceDesc.Name, serviceDesc.Version);
        }

        /// <summary>
        /// 获取指定名称的服务
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static ServiceInfo[] GetServiceInfos(this ITrayPlatform platform, string serviceName)
        {
            return platform.GetAllServiceInfos().Where(v => v.ServiceDesc.Name == serviceName).ToArray();
        }

        /// <summary>
        /// 获取所有正在运行的服务
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static ServiceDesc[] GetAllServices(this ITrayPlatform platform)
        {
            return platform.GetAllServiceInfos().ToArray(si => si.ServiceDesc);
        }

        /// <summary>
        /// 重新启动指定的服务
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="serviceDesc"></param>
        /// <param name="configuration"></param>
        /// <param name="force"></param>
        public static void RestartService(this ITrayPlatform platform, ServiceDesc serviceDesc, TrayAppServiceStartType startType, string configuration, bool force = false)
        {
            Contract.Requires(platform != null && serviceDesc != null);

            ServiceInfo sInfo = platform.GetAllServiceInfos().FirstOrDefault(v => v.ServiceDesc == serviceDesc);
            string basePath = (string)platform.GetData(SFNames.DataKeys.RUNNING_BASE_PATH);
            Guid clientId = (Guid)platform.GetData(SystemDataKeys.CLIENT_ID);

            if (sInfo != null)
            {
                if (force)
                {
                    platform.UnloadService(serviceDesc);
                    platform.LoadService(SFSettings.GetRunningMainAssemblyPath(serviceDesc, basePath, clientId), configuration, true);
                }
                else
                {
                    platform.SetServiceStatus(serviceDesc, AppServiceStatus.Running);
                }
            }
            else
            {
                platform.LoadService(SFSettings.GetRunningMainAssemblyPath(serviceDesc, basePath, clientId), configuration, true);
            }
        }

        /// <summary>
        /// 判断是否包含指定的服务
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static bool ContainsService(AppInvokeInfo[] infos, ServiceDesc serviceDesc)
        {
            Contract.Requires(infos != null && serviceDesc != null);

            var list = infos.SelectMany(v => v.ServiceDescs);
            if (serviceDesc.Version.IsEmpty)
                return list.Any(sd => sd.Name == serviceDesc.Name);

            return list.Any(sd => sd == serviceDesc);
        }

        /// <summary>
        /// 判断是否包含指定的服务
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool ContainsService(AppInvokeInfo[] infos, string serviceName)
        {
            Contract.Requires(infos != null && serviceName != null);

            return ContainsService(infos, new ServiceDesc(serviceName, SVersion.Empty));
        }

        /// <summary>
        /// 获取Master服务器的通信方式
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="masterAddressKey"></param>
        /// <returns></returns>
        public static CommunicationOption[] GetMasterCommunicationOption(this IConfiguration cfg, string masterAddressKey = "masterAddress")
        {
            Contract.Requires(cfg != null);

            CommunicationOption[] options = (cfg.Get(masterAddressKey) ?? string.Empty).Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .SelectDistinct(s => ReviseCommunicationOption(CommunicationOption.Parse(s))).ToArray();

            if (options.Length == 0)
                return new[] { new CommunicationOption("127.0.0.1:8090", CommunicationType.WTcp, false) };

            return options;
        }

        /// <summary>
        /// 修正地址，将一些占位符修正为实际的地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string ReviseAddress(string address)
        {
            switch (address)
            {
                case "self":
                    return _GetLocalIp();

                case "localhost":
                    return "127.0.0.1";
            }

            return address;
        }

        /// <summary>
        /// 修正地址，将一些占位符修正为实际的地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static ServiceAddress ReviseServiceAddress(ServiceAddress address)
        {
            if (address == null)
                return null;

            return new ServiceAddress(ReviseAddress(address.Address), address.Port);
        }

        /// <summary>
        /// 修正地址，将一些占位符修正为实际的地址
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static CommunicationOption ReviseCommunicationOption(CommunicationOption option)
        {
            if (option == null)
                return null;

            return new CommunicationOption(ReviseServiceAddress(option.Address), option.Type, option.Duplex);
        }

        private static AutoLoad<IPAddress> _localIp = new AutoLoad<IPAddress>(() => {
            IPHostEntry entity = Dns.GetHostEntry(Dns.GetHostName());
            if (entity != null && !entity.AddressList.IsNullOrEmpty())
            {
                IPAddress address = entity.AddressList.FirstOrDefault(v => !v.IsIPv6LinkLocal);
                if (address != null)
                    return address;
            }

            IPAddress[] addresses = NetworkUtility.GetAllEnableIP4Addresses();
            if (!addresses.IsNullOrEmpty())
                return addresses[0];

            return null;
        });

        private static string _GetLocalIp()
        {
            IPAddress ipAddress = _localIp.Value;
            return (ipAddress == null) ? "127.0.0.1" : ipAddress.ToString();
        }

        /// <summary>
        /// 获取Master服务器的通信方式，如果失败则返回空引用
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="masterAddressKey"></param>
        /// <returns></returns>
        public static CommunicationOption TryGetMasterCommunicationOption(this IConfiguration cfg, string masterAddressKey = "masterAddress")
        {
            Contract.Requires(cfg != null);

            return (cfg.Get(masterAddressKey) ?? string.Empty).Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => ReviseCommunicationOption(CommunicationOption.Parse(s))).FirstOrDefault();
        }

        /// <summary>
        /// 更新指定服务的配置内容
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="sd"></param>
        /// <param name="configuration"></param>
        public static bool UpdateConfiguration(this ITrayConfiguration cfg, ServiceDesc sd, string configuration)
        {
            Contract.Requires(cfg != null);

            ITrayConfiguration cfg0 = cfg.FindTrayConfiguration(sd);
            if (cfg0 != null)
            {
                cfg0.UpdateConfiguration(configuration);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 从类型中加载AppCommand
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static AppCommandCollection LoadAppCommandsFromInstance(object instance, AppCommandLoadType loadType = AppCommandLoadType.All)
        {
            Contract.Requires(instance != null);

            AppCommandCollection cmds = new AppCommandCollection();
            if (loadType.HasFlag(AppCommandLoadType.Methods))
                cmds.AddRange(_LoadAppCommandsFromInstance_FromMethods(instance));

            if (loadType.HasFlag(AppCommandLoadType.NestedClasses))
                cmds.AddRange(AppCommandCollection.LoadFromType(instance.GetType(), new object[] { instance }));

            return cmds;
        }

        private static AppCommandCollection _LoadAppCommandsFromInstance_FromMethods(object instance)
        {
            BindingFlags f = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            return _LoadFromMethods(instance.GetType().SearchMembers<AppCommandAttribute, MethodInfo, AppCommandAttribute>(
                (attrs, mInfo) => attrs[0],
                (attrs, mInfo) => mInfo as MethodInfo, f, true), instance);
        }

        private static AppCommandCollection _LoadFromMethods(IDictionary<AppCommandAttribute, MethodInfo> dict, object instance)
        {
            AppCommandCollection cmds = new AppCommandCollection();
            foreach (var item in dict)
            {
                if (item.Value == null)
                    continue;

                AppCommandAttribute attr = item.Key;
                MethodInfo mInfo = item.Value;
                ParameterInfo[] paraInfos = mInfo.GetParameters();
                if (paraInfos.Length == 0 || paraInfos.Length > 3 || paraInfos[0].ParameterType != typeof(AppCommandExecuteContext)
                    || paraInfos[paraInfos.Length - 1].ParameterType != typeof(ServiceResult).MakeByRefType())
                    throw _CreateMethodErrorException(mInfo);

                ParameterInfo retInfo = mInfo.ReturnParameter;
                AppCommandInfo cmdInfo = AppCommandAttribute.GetAppCommandInfo(mInfo);

                try
                {
                    if (paraInfos.Length == 3)
                    {
                        if (retInfo.ParameterType != typeof(void))  // 有输入输出
                        {
                            Type cmdType = typeof(AppCommandFuncWrapper<,>).MakeGenericType(paraInfos[1].ParameterType, retInfo.ParameterType);
                            Type funcType = typeof(AppCommandFunc<,>).MakeGenericType(paraInfos[1].ParameterType, retInfo.ParameterType);
                            Delegate func = Delegate.CreateDelegate(funcType, instance, mInfo);
                            AppCommandInfo info = AppCommandInfo.FromPrototype(cmdInfo, paraInfos[1].ParameterType, retInfo.ParameterType);
                            IAppCommand cmd = Activator.CreateInstance(cmdType, new object[] { info, func }) as IAppCommand;
                            cmds.Add(cmd);
                        }
                        else  // 有输入无输出
                        {
                            Type cmdType = typeof(AppCommandFuncWrapper<>).MakeGenericType(paraInfos[1].ParameterType);
                            Type funcType = typeof(AppCommandFunc<>).MakeGenericType(paraInfos[1].ParameterType);
                            Delegate func = Delegate.CreateDelegate(funcType, instance, mInfo);
                            AppCommandInfo info = AppCommandInfo.FromPrototype(cmdInfo, paraInfos[1].ParameterType);
                            IAppCommand cmd = Activator.CreateInstance(cmdType, new object[] { info, func }) as IAppCommand;
                            cmds.Add(cmd);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }

            return cmds;
        }

        #region Class AppCommandFuncWrapper ...

        delegate TOutput AppCommandFunc<TInput, TOutput>(AppCommandExecuteContext context, TInput req, ref ServiceResult sr)
            where TInput : RequestEntity
            where TOutput : ReplyEntity;

        class AppCommandFuncWrapper<TInput, TOutput> : AppCommandBase<TInput, TOutput>
            where TInput : RequestEntity
            where TOutput : ReplyEntity
        {
            public AppCommandFuncWrapper(AppCommandInfo info, AppCommandFunc<TInput, TOutput> func)
            {
                _info = info;
                _func = func;
            }

            protected override AppCommandInfo OnCreateInfo()
            {
                return _info;
            }

            private readonly AppCommandFunc<TInput, TOutput> _func;
            private readonly AppCommandInfo _info;

            protected override TOutput OnExecute(AppCommandExecuteContext context, TInput req, ref ServiceResult sr)
            {
                return _func(context, req, ref sr);
            }
        }

        delegate void AppCommandFunc<TInput>(AppCommandExecuteContext context, TInput req, ref ServiceResult sr)
            where TInput : RequestEntity;

        class AppCommandFuncWrapper<TInput> : AppCommandActionBase<TInput>
            where TInput : RequestEntity
        {
            public AppCommandFuncWrapper(AppCommandInfo info, AppCommandFunc<TInput> func)
            {
                _info = info;
                _func = func;
            }

            private readonly AppCommandInfo _info;
            private readonly AppCommandFunc<TInput> _func;

            protected override AppCommandInfo OnCreateInfo()
            {
                return _info;
            }

            protected override void OnExecute(AppCommandExecuteContext context, TInput req, ref ServiceResult sr)
            {
                _func(context, req, ref sr);
            }
        }

        #endregion

        private static ServiceException _CreateMethodErrorException(MethodInfo mInfo)
        {
            return new ServiceException(ServiceStatusCode.ServerError, string.Format("方法{0}原型错误", mInfo.DeclaringType.FullName + "." + mInfo.Name));
        }
    }

    [Flags]
    public enum AppCommandLoadType
    {
        /// <summary>
        /// 嵌套类
        /// </summary>
        NestedClasses = 0x01,

        /// <summary>
        /// 方法
        /// </summary>
        Methods = 0x02,

        /// <summary>
        /// 所有
        /// </summary>
        All = -1,
    }
}
