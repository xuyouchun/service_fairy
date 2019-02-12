using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 通信策略
    /// </summary>
    public interface ICommunicate : IDisposable
    {
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="data">请求参数</param>
        /// <param name="context">上下文环境</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null);
    }

    /// <summary>
    /// 通信策略的上下文环境
    /// </summary>
    [Serializable]
    public class CommunicateContext
    {
        public CommunicateContext(ServiceAddress from, ServiceEndPoint caller, Guid sessionId)
        {
            From = from;
            Caller = caller;
            SessionId = sessionId;
        }

        /// <summary>
        /// 调用者地址
        /// </summary>
        [DataMember]
        public ServiceAddress From { get; private set; }

        /// <summary>
        /// 调用者
        /// </summary>
        [DataMember]
        public ServiceEndPoint Caller { get; private set; }

        /// <summary>
        /// 调用堆栈的会话标识
        /// </summary>
        [DataMember]
        public Guid SessionId { get; private set; }

        // 是否为本地调用
        public bool IsLocalHost()
        {
            return From == null || From.IsLocalHost();
        }
    }

    /// <summary>
    /// 服务终端的调用方式
    /// </summary>
    public enum ServiceTargetModel : byte
    {
        /// <summary>
        /// 自动
        /// </summary>
        [Desc("自动调用")]
        Auto,

        /// <summary>
        /// 指定终端
        /// </summary>
        [Desc("指定了调用目标")]
        Direct,

        /// <summary>
        /// 多播
        /// </summary>
        [Desc("多播")]
        Broadcast,
    }

    /// <summary>
    /// 服务的终端
    /// </summary>
    [Serializable, DataContract]
    public class ServiceTarget
    {
        public ServiceTarget()
        {

        }

        public ServiceTarget(ServiceTargetModel model, ServiceEndPoint[] endpoints = null)
        {
            Model = model;
            EndPoints = endpoints;
        }

        /// <summary>
        /// 调用方式
        /// </summary>
        [DataMember]
        public ServiceTargetModel Model { get; set; }

        /// <summary>
        /// 终端列表
        /// </summary>
        [DataMember]
        public ServiceEndPoint[] EndPoints { get; set; }

        /// <summary>
        /// 指定ClientId的单一终端
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static ServiceTarget FromClientId(Guid clientId)
        {
            return new ServiceTarget(ServiceTargetModel.Direct, new[] { new ServiceEndPoint(clientId) });
        }

        /// <summary>
        /// 指定ClientId的多个终端
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        public static ServiceTarget FromClientIds(Guid[] clientIds)
        {
            return new ServiceTarget(ServiceTargetModel.Broadcast, clientIds.ToArray(id => new ServiceEndPoint(id)));
        }

        /// <summary>
        /// 指定服务名称的多个终端
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static ServiceTarget FromService(ServiceDesc serviceDesc, ServiceTargetModel model = ServiceTargetModel.Broadcast)
        {
            Contract.Requires(serviceDesc != null);
            return new ServiceTarget(model, new[] { new ServiceEndPoint(Guid.Empty, serviceDesc) });
        }

        /// <summary>
        /// 指定多个服务名称的多个终端
        /// </summary>
        /// <param name="serviceDescs"></param>
        /// <returns></returns>
        public static ServiceTarget FromServices(ServiceDesc[] serviceDescs)
        {
            Contract.Requires(serviceDescs != null);
            return new ServiceTarget(ServiceTargetModel.Broadcast, serviceDescs.ToArray(sd => new ServiceEndPoint(Guid.Empty, sd)));
        }

        /// <summary>
        /// 同时指定终端ID与服务名称
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public static ServiceTarget FromServiceEndPoint(Guid clientId, ServiceDesc serviceDesc)
        {
            return FromServiceEndPoints(new[] { new ServiceEndPoint(clientId, serviceDesc) });
        }

        /// <summary>
        /// 指定终端服务
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static ServiceTarget FromServiceEndPoint(ServiceEndPoint endpoint)
        {
            Contract.Requires(endpoint != null);
            return FromServiceEndPoints(new[] { endpoint });
        }

        /// <summary>
        /// 指定多个终端服务
        /// </summary>
        /// <param name="endpoints"></param>
        /// <returns></returns>
        public static ServiceTarget FromServiceEndPoints(ServiceEndPoint[] endpoints)
        {
            Contract.Requires(endpoints != null);
            return new ServiceTarget(_GetTargetModel(endpoints), endpoints);
        }

        private static ServiceTargetModel _GetTargetModel(ServiceEndPoint[] endpoints)
        {
            if (endpoints.IsNullOrEmpty() || (endpoints.Length == 1 && endpoints[0].ClientId == Guid.Empty))
                return ServiceTargetModel.Auto;

            if (endpoints.Length != 1)
                return ServiceTargetModel.Broadcast;

            ServiceEndPoint ep = endpoints[0];
            return ep.ClientId != Guid.Empty ? ServiceTargetModel.Direct : ServiceTargetModel.Broadcast;
        }

        public static readonly ServiceTarget Default = new ServiceTarget(ServiceTargetModel.Auto);

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return EndPoints.IsNullOrEmpty();
        }
    }

    /// <summary>
    /// 调用设置
    /// </summary>
    [Serializable, DataContract]
    public class CallingSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invokeType">通信方式</param>
        /// <param name="model">调用方式</param>
        /// <param name="target">调用目标</param>
        /// <param name="sid">安全码</param>
        public CallingSettings(CommunicateInvokeType invokeType = CommunicateInvokeType.RequestReply, Sid sid = default(Sid),
            ServiceTarget target = null, int tryTimes = 1)
        {
            InvokeType = invokeType;
            Target = target;
            Sid = sid;
            TryTimes = Math.Max(1, tryTimes);
        }

        /// <summary>
        /// 是否等待应答
        /// </summary>
        [DataMember]
        public CommunicateInvokeType InvokeType { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; set; }

        

        /// <summary>
        /// 调用目标
        /// </summary>
        [DataMember]
        public ServiceTarget Target { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        [DataMember]
        public int TryTimes { get; set; }

        /// <summary>
        /// 是否需要应答
        /// </summary>
        /// <returns></returns>
        public bool NeedReply()
        {
            return GetDirection() == CommunicateInvokeType.RequestReply;
        }

        /// <summary>
        /// 获取通信方向的部分
        /// </summary>
        /// <returns></returns>
        public CommunicateInvokeType GetDirection()
        {
            return InvokeType & (CommunicateInvokeType)0xFF;
        }

        /// <summary>
        /// 单向请求应答
        /// </summary>
        public static readonly CallingSettings RequestReply = new CallingSettings(CommunicateInvokeType.RequestReply);

        /// <summary>
        /// 单向通信的默认设置
        /// </summary>
        public static readonly CallingSettings OneWay = new CallingSettings(CommunicateInvokeType.OneWay);

        /// <summary>
        /// 判断一个调用设置是否为默认
        /// </summary>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public static bool IsDefault(CallingSettings settings)
        {
            if (settings == null)
                return true;

            return settings.InvokeType == CommunicateInvokeType.RequestReply
                && settings.Sid.IsEmpty() && (settings.Target == null || settings.Target.EndPoints.IsNullOrEmpty());
        }

        /// <summary>
        /// 带有安全码的默认设置
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <returns>调用设置</returns>
        public static CallingSettings RequestReplyWithSid(Sid sid)
        {
            return new CallingSettings(invokeType: CommunicateInvokeType.RequestReply, sid: sid);
        }

        /// <summary>
        /// 带有重试次数的默认设置
        /// </summary>
        /// <param name="tryTimes">尝试次数</param>
        /// <param name="sid">安全码</param>
        /// <returns>调用设置</returns>
        public static CallingSettings RequestReplyWithTryTimes(int tryTimes, Sid sid = default(Sid))
        {
            return new CallingSettings(invokeType: CommunicateInvokeType.RequestReply, tryTimes: tryTimes, sid: sid);
        }

        /// <summary>
        /// 有安全码的无应答默认设置
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <returns>调用设置</returns>
        public static CallingSettings OneWayWithSid(Sid sid)
        {
            return new CallingSettings(invokeType: CommunicateInvokeType.OneWay, sid: sid);
        }

        /// <summary>
        /// 指定调用终端
        /// </summary>
        /// <param name="endpoints">服务终端</param>
        /// <param name="sid">安全码</param>
        /// <param name="invokeType">交互类型</param>
        /// <returns>调用设置</returns>
        public static CallingSettings FromTarget(ServiceEndPoint[] endpoints,
            CommunicateInvokeType invokeType = CommunicateInvokeType.RequestReply, Sid sid = default(Sid))
        {
            Contract.Requires(endpoints != null);
            return FromTarget(ServiceTarget.FromServiceEndPoints(endpoints), invokeType, sid);
        }

        /// <summary>
        /// 指定了调用终端
        /// </summary>
        /// <param name="target">调用目标</param>
        /// <param name="invokeType">交互方式</param>
        /// <param name="sid">安全码</param>
        /// <returns>调用设置</returns>
        public static CallingSettings FromTarget(ServiceTarget target,
            CommunicateInvokeType invokeType = CommunicateInvokeType.RequestReply, Sid sid = default(Sid))
        {
            Contract.Requires(target != null);
            return new CallingSettings(invokeType, sid, target);
        }

        /// <summary>
        /// 指定调用终端
        /// </summary>
        /// <param name="endpoint">终端</param>
        /// <param name="sid">安全码</param>
        /// <param name="invokeType">通信方式</param>
        /// <returns>调用设置</returns>
        public static CallingSettings FromTarget(ServiceEndPoint endpoint,
            CommunicateInvokeType invokeType = CommunicateInvokeType.RequestReply, Sid sid = default(Sid))
        {
            Contract.Requires(endpoint != null);

            return FromTarget(new[] { endpoint }, invokeType, sid);
        }

        /// <summary>
        /// 指定调用终端
        /// </summary>
        /// <param name="clientId">终端ID</param>
        /// <param name="sid">安全码</param>
        /// <param name="invokeType">通信方式</param>
        /// <returns>调用设置</returns>
        public static CallingSettings FromTarget(Guid clientId, 
            CommunicateInvokeType invokeType = CommunicateInvokeType.RequestReply, Sid sid = default(Sid))
        {
            return FromTarget(new ServiceEndPoint(clientId), invokeType, sid);
        }

        /// <summary>
        /// 指定调用终端
        /// </summary>
        /// <param name="clientId">终端ID</param>
        /// <param name="serviceDesc">服务</param>
        /// <param name="invokeType">通信方式</param>
        /// <param name="sid">安全码</param>
        /// <returns>调用设置</returns>
        public static CallingSettings FromTarget(Guid clientId, ServiceDesc serviceDesc, 
            CommunicateInvokeType invokeType = CommunicateInvokeType.RequestReply, Sid sid = default(Sid))
        {
            return FromTarget(new ServiceEndPoint(clientId, serviceDesc), invokeType, sid);
        }

        /// <summary>
        /// 指定了服务
        /// </summary>
        /// <param name="serviceDesc">服务</param>
        /// <param name="invokeType">交互方式</param>
        /// <param name="sid">安全码</param>
        /// <param name="model">调用模式</param>
        /// <returns>调用设置</returns>
        public static CallingSettings FromTarget(ServiceDesc serviceDesc,
            CommunicateInvokeType invokeType = CommunicateInvokeType.RequestReply, Sid sid = default(Sid), ServiceTargetModel model = ServiceTargetModel.Auto)
        {
            return FromTarget(ServiceTarget.FromService(serviceDesc, model), invokeType, sid);
        }

        /// <summary>
        /// 指定了服务
        /// </summary>
        /// <param name="serviceDesc">服务</param>
        /// <param name="invokeType">交互方式</param>
        /// <param name="sid">安全码</param>
        /// <param name="model">调用模式</param>
        /// <returns>调用设置</returns>
        public static CallingSettings FromTarget(string serviceDesc,
            CommunicateInvokeType invokeType = CommunicateInvokeType.RequestReply, Sid sid = default(Sid), ServiceTargetModel model = ServiceTargetModel.Auto)
        {
            return FromTarget(ServiceDesc.Parse(serviceDesc), invokeType: invokeType, sid: sid, model: model);
        }

        /// <summary>
        /// 从原型创建
        /// </summary>
        /// <param name="prototype">原型</param>
        /// <param name="sid">安全码</param>
        /// <returns>调用设置</returns>
        public static CallingSettings FromPrototype(CallingSettings prototype, Sid sid)
        {
            if (prototype == null)
                prototype = RequestReply;

            return new CallingSettings(invokeType: prototype.InvokeType,
                sid: !sid.IsEmpty() ? sid : prototype.Sid, target: prototype.Target
            );
        }

        /// <summary>
        /// 从原型创建
        /// </summary>
        /// <param name="prototype">原型</param>
        /// <param name="invokeType">交互方式</param>
        /// <returns>调用设置</returns>
        public static CallingSettings FromPrototype(CallingSettings prototype, CommunicateInvokeType invokeType)
        {
            if (prototype == null)
                prototype = RequestReply;

            return new CallingSettings(
                invokeType: invokeType, sid: prototype.Sid, target: prototype.Target
            );
        }

        /// <summary>
        /// 转换为正常调用模式
        /// </summary>
        /// <returns>调用设置</returns>
        public CallingSettings ToNormal()
        {
            return new CallingSettings(InvokeType, Sid, null);
        }
    }

    /// <summary>
    /// 调用方式
    /// </summary>
    [Flags]
    public enum CommunicateInvokeType
    {
        /// <summary>
        /// 请求应答
        /// </summary>
        [Desc("请求应答")]
        RequestReply = 0x00,

        /// <summary>
        /// 单向无应答
        /// </summary>
        [Desc("单向无应答")]
        OneWay = 0x01,

        /// <summary>
        /// 打开流连接
        /// </summary>
        [Desc("流连接")]
        OpenStream = 0x02,

        /// <summary>
        /// 使用原始的安全码
        /// </summary>
        [Desc("使用原始的安全码")]
        UseOriginalSid = 0x01 << 8,
    }
}
