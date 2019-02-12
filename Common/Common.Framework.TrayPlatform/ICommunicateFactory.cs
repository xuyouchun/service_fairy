using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common.Communication.Wcf;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 通信通道的创建工厂
    /// </summary>
    public interface ICommunicateFactory
    {
        /// <summary>
        /// 创建指定目标的通道
        /// </summary>
        /// <param name="target"></param>
        /// <param name="includeMySelf">是否包含自身</param>
        /// <param name="async">是否为异步调用</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        ICommunicate CreateCommunicate(ServiceTarget target, bool includeMySelf = false, bool async = true, ICommunicateCallback callback = null);

        /// <summary>
        /// 创建指定地址的通道
        /// </summary>
        /// <param name="options">通道</param>
        /// <param name="callback">回调</param>
        /// <param name="async">是否为异步调用</param>
        /// <returns></returns>
        ICommunicate CreateCommunicate(CommunicationOption[] options, bool async = true, ICommunicateCallback callback = null);

        /// <summary>
        /// 拥有者
        /// </summary>
        ServiceDesc Owner { get; set; }
    }

    /// <summary>
    /// 多播回调
    /// </summary>
    public interface ICommunicateCallback
    {
        /// <summary>
        /// 回调，返回值表示是否取消调用，只有在同步多播方式中，该返回值才会起作用
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        bool Callback(CommunicateCallbackArgs args);
    }

    /// <summary>
    /// 多播回调的参数
    /// </summary>
    [Serializable, DataContract]
    public class CommunicateCallbackArgs
    {
        public CommunicateCallbackArgs(CommunicateData replyData)
        {
            ReplyData = replyData;
        }

        /// <summary>
        /// 应答数据
        /// </summary>
        [DataMember]
        public CommunicateData ReplyData { get; private set; }
    }

    /// <summary>
    /// 用于寻找指定服务的信道
    /// </summary>
    public interface IServiceCommunicationSearcher
    {
        /// <summary>
        /// 寻找指定服务的信道
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        AppInvokeInfo[] Search(ServiceDesc serviceDesc);

        /// <summary>
        /// 寻找指定终端的信道
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        AppInvokeInfo[] Search(Guid[] clientIds);

        /// <summary>
        /// 寻找所有终端的信道
        /// </summary>
        /// <returns></returns>
        AppInvokeInfo[] SearchAll();
    }

    public static class CommunicateFactoryUtility
    {
        /// <summary>
        /// 创建默认的通道
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ICommunicate CreateDefaultCommunicate(this ICommunicateFactory factory)
        {
            Contract.Requires(factory != null);

            return factory.CreateCommunicate(ServiceTarget.Default);
        }

        /// <summary>
        /// 创建指定终端的通道
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static ICommunicate CreateCommunicate(this ICommunicateFactory factory, Guid clientId)
        {
            Contract.Requires(factory != null);

            return factory.CreateCommunicate(ServiceTarget.FromClientId(clientId));
        }

        /// <summary>
        /// 创建指定终端的广播通道
        /// </summary>
        /// <param name="facotry"></param>
        /// <param name="clientIds"></param>
        /// <param name="async"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ICommunicate CreateBroadcastCommunicate(this ICommunicateFactory facotry, Guid[] clientIds,
            bool async = true, ICommunicateCallback callback = null)
        {
            Contract.Requires(facotry != null && clientIds != null);

            return facotry.CreateCommunicate(ServiceTarget.FromClientIds(clientIds), includeMySelf: true, async: async, callback: callback);
        }

        /// <summary>
        /// 创建指定服务的广播通道
        /// </summary>
        /// <param name="facotry"></param>
        /// <param name="serviceDesc"></param>
        /// <param name="includeMySelf"></param>
        /// <param name="async"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ICommunicate CreateBroadcastCommunicate(this ICommunicateFactory facotry, ServiceDesc serviceDesc,
            bool includeMySelf = false, bool async = true, ICommunicateCallback callback = null)
        {
            Contract.Requires(facotry != null && serviceDesc != null);
            return facotry.CreateCommunicate(ServiceTarget.FromService(serviceDesc), includeMySelf: includeMySelf, async: async, callback: callback);
        }

        /// <summary>
        /// 创建指定服务的广播通道
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="serviceDescs"></param>
        /// <param name="includeMySelf"></param>
        /// <param name="async"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ICommunicate CreateBroadcastCommunicate(this ICommunicateFactory factory, ServiceDesc[] serviceDescs,
            bool async = true, bool includeMySelf = false, ICommunicateCallback callback = null)
        {
            Contract.Requires(factory != null && serviceDescs != null);
            return factory.CreateCommunicate(ServiceTarget.FromServices(serviceDescs), async: async, includeMySelf: includeMySelf, callback: callback);
        }

        /// <summary>
        /// 创建指定服务终端的广播通道
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="endpoints"></param>
        /// <param name="async"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ICommunicate CreateBroadcastCommunicate(this ICommunicateFactory factory, ServiceEndPoint[] endpoints,
            bool async = true, ICommunicateCallback callback = null)
        {
            Contract.Requires(factory != null && endpoints != null);
            return factory.CreateCommunicate(ServiceTarget.FromServiceEndPoints(endpoints), async: async, includeMySelf: true, callback: callback);
        }

        /// <summary>
        /// 创建指定服务终端的广播通道
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="endpoint"></param>
        /// <param name="async"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ICommunicate CreateBroadcastCommunicate(this ICommunicateFactory factory, ServiceEndPoint endpoint,
            bool async = true, ICommunicateCallback callback = null)
        {
            Contract.Requires(factory != null && endpoint != null);
            return factory.CreateCommunicate(ServiceTarget.FromServiceEndPoint(endpoint), includeMySelf: true, async: true, callback: callback);
        }
    }
}
