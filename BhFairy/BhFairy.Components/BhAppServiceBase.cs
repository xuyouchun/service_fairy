using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Components;
using System.Diagnostics.Contracts;
using BhFairy.ApplicationInvoke;
using Common.Contracts.Service;
using Common.Communication.Wcf;

namespace BhFairy.Components
{
    /// <summary>
    /// BhFairy中AppService的基类
    /// </summary>
    public class BhAppServiceBase : SystemAppServiceBase
    {
        public BhAppServiceBase()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            Invoker = new ApplicationInvoker(base.Invoker);
        }

        /// <summary>
        /// 默认的服务调用者
        /// </summary>
        public new ApplicationInvoker Invoker { get; private set; }

        /// <summary>
        /// 创建SystemInvoker
        /// </summary>
        /// <param name="communicate"></param>
        /// <returns></returns>
        public new ApplicationInvoker CreateInvoker(ICommunicate communicate)
        {
            Contract.Requires(communicate != null);
            return CreateInvoker(Context.CreateServiceClient(communicate));
        }

        /// <summary>
        /// 根据ServiceClient创建SystemInvoker
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <returns></returns>
        public new ApplicationInvoker CreateInvoker(IServiceClient serviceClient)
        {
            Contract.Requires(serviceClient != null);
            return ApplicationInvoker.FromServiceClient(serviceClient);
        }

        /// <summary>
        /// 创建指定插件的SystemInvoker
        /// </summary>
        /// <param name="addin"></param>
        /// <returns></returns>
        public new ApplicationInvoker CreateInvoker(IAppServiceAddin addin)
        {
            Contract.Requires(addin != null);
            return CreateInvoker(Context.CreateServiceClient(addin));
        }

        /// <summary>
        /// 创建指定终端的SystemInvoker
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public new ApplicationInvoker CreateInvoker(Guid clientId)
        {
            Contract.Requires(clientId != Guid.Empty);
            return CreateInvoker(Context.CreateServiceClient(clientId));
        }

        /// <summary>
        /// 创建指定服务终端的SystemInvoker
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public new ApplicationInvoker CreateInvoker(ServiceEndPoint endpoint)
        {
            Contract.Requires(endpoint != null);
            return CreateInvoker(Context.CreateServiceClient(endpoint));
        }

        /// <summary>
        /// 创建指定通信方式的SystemInvoker
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public new ApplicationInvoker CreateInvoker(CommunicationOption option)
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
        public new ApplicationInvoker CreateInvoker(int userId, bool throwError = false)
        {
            IServiceClient serviceClient = Context.CreateServiceClient(userId, throwError);
            if (serviceClient == null)
                return null;

            return CreateInvoker(serviceClient);
        }
    }
}
