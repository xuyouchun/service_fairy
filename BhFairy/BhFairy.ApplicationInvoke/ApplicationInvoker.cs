using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Package.Service;
using ServiceFairy;
using Common.Contracts.Service;
using Common.Communication.Wcf;
using Common.Contracts;

namespace BhFairy.ApplicationInvoke
{
    /// <summary>
    /// 对应用服务调用的封装
    /// </summary>
    public partial class ApplicationInvoker : SystemInvoker
    {
        public ApplicationInvoker(InvokerBase innerInvoker)
            : base(innerInvoker)
        {

        }

        public ApplicationInvoker(IServiceClientProvider scProvider, bool disposeIt = false)
            : base(scProvider, disposeIt)
        {
            
        }

        /// <summary>
        /// 通过ServiceClient创建
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="disposeIt"></param>
        /// <returns></returns>
        public static new ApplicationInvoker FromServiceClient(IServiceClient serviceClient, bool disposeIt = false)
        {
            return new ApplicationInvoker(CreateProviderByServiceClient(serviceClient), disposeIt);
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="navigation"></param>
        /// <returns></returns>
        public static new ApplicationInvoker FromNavigation(string navigation)
        {
            return FromNavigation(CommunicationOption.Parse(navigation));
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="navigation"></param>
        /// <returns></returns>
        public static new ApplicationInvoker FromNavigation(CommunicationOption navigation)
        {
            return new ApplicationInvoker(new SystemConnection(navigation), true);
        }

        /// <summary>
        /// 通过通道创建
        /// </summary>
        /// <param name="communicate"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static new ApplicationInvoker FromCommunicate(ICommunicate communicate, DataFormat format = DataFormat.Unknown)
        {
            return new ApplicationInvoker(CreateProviderByCommunicate(communicate, format), true);
        }

        /// <summary>
        /// 通过代理创建
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static new ApplicationInvoker FromProxy(CommunicationOption proxy, DataFormat format = DataFormat.Unknown)
        {
            return new ApplicationInvoker(CreateProviderByProxy(proxy, format), true);
        }

        /// <summary>
        /// 通过代理创建
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static new ApplicationInvoker FromProxy(string proxy, DataFormat format = DataFormat.Unknown)
        {
            return FromProxy(CommunicationOption.Parse(proxy), format);
        }
    }
}
