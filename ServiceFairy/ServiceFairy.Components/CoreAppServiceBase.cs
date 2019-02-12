using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;
using Common.Communication.Wcf;
using Common.Framework.TrayPlatform;
using ServiceFairy.Entities.Security;
using Common;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 核心服务的基类
    /// </summary>
    public abstract class CoreAppServiceBase : TrayAppServiceBaseEx
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            Invoker = CoreInvoker.FromServiceClient(Context.ServiceClient);

            AppComponentManager.AddRange(new IAppComponent[] {
                ServiceEvent = new ServiceEventAppComponent(this),
                ServiceAddIn = new ServiceAddinAppComponent(this),
                ServiceWatch = new ServiceWatchAppComponent(this),
                CryptoExecutor = new CryptoExecutorAppComponent(this),
                SecurityManager = new SecurityManagerAppComponent(this),
                CloudManager = new CloudManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 服务事件的客户端管理器
        /// </summary>
        public ServiceEventAppComponent ServiceEvent { get; private set; }

        /// <summary>
        /// 插件管理器
        /// </summary>
        public ServiceAddinAppComponent ServiceAddIn { get; private set; }

        /// <summary>
        /// 服务监控器终端
        /// </summary>
        public ServiceWatchAppComponent ServiceWatch { get; private set; }

        /// <summary>
        /// 加密解密运算器
        /// </summary>
        public CryptoExecutorAppComponent CryptoExecutor { get; private set; }

        /// <summary>
        /// 安全信息管理器
        /// </summary>
        public SecurityManagerAppComponent SecurityManager { get; private set; }

        /// <summary>
        /// 云平台管理器
        /// </summary>
        public CloudManagerAppComponent CloudManager { get; private set; }

        /// <summary>
        /// 核心服务的调用
        /// </summary>
        public CoreInvoker Invoker { get; set; }

        /// <summary>
        /// 创建SystemInvoker
        /// </summary>
        /// <param name="communicate"></param>
        /// <returns></returns>
        public CoreInvoker CreateInvoker(ICommunicate communicate)
        {
            Contract.Requires(communicate != null);
            return CreateInvoker(Context.CreateServiceClient(communicate));
        }

        /// <summary>
        /// 根据ServiceClient创建SystemInvoker
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <returns></returns>
        public CoreInvoker CreateInvoker(IServiceClient serviceClient)
        {
            Contract.Requires(serviceClient != null);
            return CoreInvoker.FromServiceClient(serviceClient);
        }

        /// <summary>
        /// 创建指定插件的SystemInvoker
        /// </summary>
        /// <param name="addin"></param>
        /// <returns></returns>
        public CoreInvoker CreateInvoker(IAppServiceAddin addin)
        {
            Contract.Requires(addin != null);
            return CreateInvoker(Context.CreateServiceClient(addin));
        }

        /// <summary>
        /// 创建指定终端的SystemInvoker
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public CoreInvoker CreateInvoker(Guid clientId)
        {
            Contract.Requires(clientId != Guid.Empty);
            return CreateInvoker(Context.CreateServiceClient(clientId));
        }

        /// <summary>
        /// 创建指定服务终端的SystemInvoker
        /// </summary>
        /// <param name="endpoint">服务终端</param>
        /// <returns></returns>
        public CoreInvoker CreateInvoker(ServiceEndPoint endpoint)
        {
            Contract.Requires(endpoint != null);
            return CreateInvoker(Context.CreateServiceClient(endpoint));
        }

        /// <summary>
        /// 创建指定通信方式的SystemInvoker
        /// </summary>
        /// <param name="option">通信设置</param>
        /// <returns></returns>
        public CoreInvoker CreateInvoker(CommunicationOption option)
        {
            Contract.Requires(option != null);
            return CreateInvoker(Context.CreateServiceClient(option));
        }

        /// <summary>
        /// 创建指定用户的连接
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="throwError">是否在出现错误时抛出异常</param>
        /// <returns></returns>
        public CoreInvoker CreateInvoker(int userId, bool throwError = false)
        {
            IServiceClient serviceClient = Context.CreateServiceClient(userId, throwError);
            if (serviceClient == null)
                return null;

            return CreateInvoker(serviceClient);
        }

        /// <summary>
        /// 获取安全信息
        /// </summary>
        /// <param name="context">上下文环境</param>
        /// <param name="method">方法</param>
        /// <returns>安全信息</returns>
        protected override SecurityInfo GetServiceSecurityInfo(CommunicateContext context, string method)
        {
            string username = Context.ServiceDesc.Name, password = "";
            return SecurityManager.Login(username, password);
        }

        /// <summary>
        /// 获取指定安全码的安全信息
        /// </summary>
        /// <param name="sids">安全码</param>
        /// <returns>安全信息</returns>
        protected override SecurityInfo[] GetSecurityInfos(Sid[] sids)
        {
            if (sids.IsNullOrEmpty())
                return Array<SecurityInfo>.Empty;

            return SecurityManager.GetSecurityInfos(sids);
        }
    }
}
