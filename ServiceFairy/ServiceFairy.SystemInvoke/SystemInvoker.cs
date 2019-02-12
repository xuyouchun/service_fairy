using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Client;
using Common.Contracts;
using Common.Communication.Wcf;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 系统服务的调用
    /// </summary>
    public partial class SystemInvoker : CoreInvoker
    {
        public SystemInvoker(InvokerBase innerInvoker, object eventSender = null)
            : base(innerInvoker, eventSender)
        {

        }

        public SystemInvoker(IServiceClientProvider scProvider, bool disposeIt = false, object eventSender = null)
            : base(scProvider, disposeIt, eventSender)
        {

        }

        /// <summary>
        /// 通过ServiceClient创建
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="disposeIt"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static new SystemInvoker FromServiceClient(IServiceClient serviceClient, bool disposeIt = false, object eventSender = null)
        {
            return new SystemInvoker(CreateProviderByServiceClient(serviceClient), disposeIt, eventSender);
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static new SystemInvoker FromNavigation(string navigation, object eventSender = null)
        {
            return FromNavigation(CommunicationOption.Parse(navigation), eventSender);
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static new SystemInvoker FromNavigation(CommunicationOption navigation, object eventSender = null)
        {
            return new SystemInvoker(new SystemConnection(navigation), true, eventSender);
        }

        /// <summary>
        /// 通过通道创建
        /// </summary>
        /// <param name="communicate"></param>
        /// <param name="format"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static new SystemInvoker FromCommunicate(ICommunicate communicate, DataFormat format = DataFormat.Unknown, object eventSender = null)
        {
            return new SystemInvoker(CreateProviderByCommunicate(communicate, format), true, eventSender);
        }

        /// <summary>
        /// 通过代理创建
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="format"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static new SystemInvoker FromProxy(CommunicationOption proxy, DataFormat format = DataFormat.Unknown, object eventSender = null)
        {
            return new SystemInvoker(CreateProviderByProxy(proxy, format), true, eventSender);
        }

        /// <summary>
        /// 通过代理创建
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="format"></param>
        /// <param name="eventSender"></param>
        /// <returns></returns>
        public static new SystemInvoker FromProxy(string proxy, DataFormat format = DataFormat.Unknown, object eventSender = null)
        {
            return FromProxy(CommunicationOption.Parse(proxy), format, eventSender);
        }
    }
}
