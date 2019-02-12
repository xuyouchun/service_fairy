using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.SystemInvoke;
using System.Diagnostics.Contracts;
using Common.Framework.TrayPlatform;
using Common.Communication.Wcf;
using ServiceFairy.Entities;
using Common.Data.UnionTable;
using ServiceFairy.DbEntities;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 含有大量组件功能的AppService基类
    /// </summary>
    public abstract class SystemAppServiceBase : CoreAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            Invoker = new SystemInvoker(base.Invoker);

            AppComponentManager.AddRange(new IAppComponent[] {
                UserManager = new UserManagerAppComponent(this),
                CacheChainManager = new CacheChainManagerAppComponent(this),
                DbConnectionManager = new DbConnectionManagerAppComponent(this, DbEntityUtility.LoadReviseInfo()),
                DistributedController = new DistributedControllerAppComponent(this),
                UserParser = new UserParserAppComponent(this),
                GroupManager = new GroupManagerAppComponent(this),
                MessageSender = new MessageSenderAppComponent(this),
            });

            LoadAppMessagesFromAssembly(typeof(ServiceEventNames).Assembly);
        }

        /// <summary>
        /// 用户管理器
        /// </summary>
        public UserManagerAppComponent UserManager { get; private set; }

        /// <summary>
        /// 用户组管理器
        /// </summary>
        public GroupManagerAppComponent GroupManager { get; set; }

        /// <summary>
        /// 分布式控制器
        /// </summary>
        public DistributedControllerAppComponent DistributedController { get; private set; }

        /// <summary>
        /// 用户组解析器
        /// </summary>
        public UserParserAppComponent UserParser { get; private set; }

        /// <summary>
        /// 缓存链管理器
        /// </summary>
        public CacheChainManagerAppComponent CacheChainManager { get; private set; }

        /// <summary>
        /// 消息发送器
        /// </summary>
        public MessageSenderAppComponent MessageSender { get; private set; }

        /// <summary>
        /// 数据库连接管理器
        /// </summary>
        public DbConnectionManagerAppComponent DbConnectionManager { get; private set; }

        /// <summary>
        /// 默认的服务调用者
        /// </summary>
        public new SystemInvoker Invoker { get; private set; }

        /// <summary>
        /// 创建SystemInvoker
        /// </summary>
        /// <param name="communicate"></param>
        /// <returns></returns>
        public new SystemInvoker CreateInvoker(ICommunicate communicate)
        {
            Contract.Requires(communicate != null);
            return CreateInvoker(Context.CreateServiceClient(communicate));
        }

        /// <summary>
        /// 根据ServiceClient创建SystemInvoker
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <returns></returns>
        public new SystemInvoker CreateInvoker(IServiceClient serviceClient)
        {
            Contract.Requires(serviceClient != null);
            return SystemInvoker.FromServiceClient(serviceClient);
        }

        /// <summary>
        /// 创建指定插件的SystemInvoker
        /// </summary>
        /// <param name="addin"></param>
        /// <returns></returns>
        public new SystemInvoker CreateInvoker(IAppServiceAddin addin)
        {
            Contract.Requires(addin != null);
            return CreateInvoker(Context.CreateServiceClient(addin));
        }

        /// <summary>
        /// 创建指定终端的SystemInvoker
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public new SystemInvoker CreateInvoker(Guid clientId)
        {
            Contract.Requires(clientId != Guid.Empty);
            return CreateInvoker(Context.CreateServiceClient(clientId));
        }

        /// <summary>
        /// 创建指定服务终端的SystemInvoker
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public new SystemInvoker CreateInvoker(ServiceEndPoint endpoint)
        {
            Contract.Requires(endpoint != null);
            return CreateInvoker(Context.CreateServiceClient(endpoint));
        }

        /// <summary>
        /// 创建指定通信方式的SystemInvoker
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public new SystemInvoker CreateInvoker(CommunicationOption option)
        {
            Contract.Requires(option != null);
            return CreateInvoker(Context.CreateServiceClient(option));
        }

        /// <summary>
        /// 创建指定用户的连接
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public new SystemInvoker CreateInvoker(int userId, bool throwError = false)
        {
            IServiceClient serviceClient = Context.CreateServiceClient(userId, throwError);
            if (serviceClient == null)
                return null;

            return CreateInvoker(serviceClient);
        }
    }
}
