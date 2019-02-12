using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using Common.Contracts;
using Common.Contracts.Service;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 核心服务的调用
    /// </summary>
    public partial class CoreInvoker : InvokerBase
    {
        public CoreInvoker(InvokerBase innerInvoker, object eventSender = null)
            : base(innerInvoker, eventSender)
        {

        }

        public CoreInvoker(IServiceClientProvider scProvider, bool disposeIt = false, object eventSender = null)
            : base(scProvider, disposeIt, eventSender)
        {

        }

        /// <summary>
        /// 通过ServiceClient创建
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <returns></returns>
        public static CoreInvoker FromServiceClient(IServiceClient serviceClient, bool disposeIt = false, object eventSender = null)
        {
            return new CoreInvoker(CreateProviderByServiceClient(serviceClient), disposeIt, eventSender);
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static CoreInvoker FromNavigation(string navigation, object eventSender = null)
        {
            return FromNavigation(CommunicationOption.Parse(navigation), eventSender);
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static CoreInvoker FromNavigation(CommunicationOption navigation, object eventSender = null)
        {
            return new CoreInvoker(new SystemConnection(navigation), true, eventSender);
        }

        /// <summary>
        /// 通过通道创建
        /// </summary>
        /// <param name="communicate"></param>
        /// <param name="format"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static CoreInvoker FromCommunicate(ICommunicate communicate, DataFormat format = DataFormat.Unknown, object eventSender = null)
        {
            return new CoreInvoker(CreateProviderByCommunicate(communicate, format), true, eventSender);
        }

        /// <summary>
        /// 通过代理创建
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="format"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static CoreInvoker FromProxy(CommunicationOption proxy, DataFormat format = DataFormat.Unknown, object eventSender = null)
        {
            return new CoreInvoker(CreateProviderByProxy(proxy, format), true, eventSender);
        }

        /// <summary>
        /// 通过代理创建
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="format"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static CoreInvoker FromProxy(string proxy, DataFormat format = DataFormat.Unknown, object eventSender = null)
        {
            return FromProxy(CommunicationOption.Parse(proxy), format, eventSender);
        }
    }
}
