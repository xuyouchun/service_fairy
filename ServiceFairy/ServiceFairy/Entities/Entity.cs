using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using System.Collections;
using System.IO;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 实体的基类
    /// </summary>
    [Serializable, DataContract]
    public class Entity
    {
        /// <summary>
        /// 验证实体的合法性
        /// </summary>
        public virtual void Validate()
        {

        }
    }

    /// <summary>
    /// 请求实体的基类
    /// </summary>
    [Serializable, DataContract]
    public class RequestEntity : Entity
    {

    }

    /// <summary>
    /// 带有终端调用都标识信息的基类
    /// </summary>
    [Serializable, DataContract]
    public class ClientRequestEntity : RequestEntity
    {
        /// <summary>
        /// 调用者ClientID
        /// </summary>
        [DataMember]
        public Guid ClientID { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (ClientID == default(Guid))
                throw new ServiceException(ServerErrorCode.ArgumentError, "未指定请求参数中的服务终端唯一标识ClientID");
        }
    }

    /// <summary>
    /// 带有服务调用者标识信息的基类
    /// </summary>
    [Serializable, DataContract]
    public class ServiceRequestEntity : ClientRequestEntity
    {
        /// <summary>
        /// 发起请求的服务信息
        /// </summary>
        [DataMember]
        public ServiceDesc Caller { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (Caller == null)
                throw new ServiceException(ServerErrorCode.ArgumentError, "未指定请求参数中的调用者Caller");
        }
    }

    /// <summary>
    /// 应答实体的基类
    /// </summary>
    [Serializable, DataContract]
    public class ReplyEntity : Entity
    {

    }

    /// <summary>
    /// 带有输入流的请求实体基类
    /// </summary>
    [Serializable, DataContract]
    public class StreamRequestEntity : RequestEntity
    {
        /// <summary>
        /// 输入流
        /// </summary>
        [IgnoreDataMember]
        public Stream Stream { get; set; }
    }

    /// <summary>
    /// 带有输出流的应答实体基类
    /// </summary>
    [Serializable, DataContract]
    public class StreamReplyEntity : ReplyEntity
    {
        /// <summary>
        /// 输出流
        /// </summary>
        [IgnoreDataMember]
        public Stream Stream { get; set; }
    }

    /// <summary>
    /// 消息通知实体基类
    /// </summary>
    [Serializable, DataContract]
    public class MessageEntity : Entity
    {
        
    }

    /// <summary>
    /// 事件参数实体基类
    /// </summary>
    [Serializable, DataContract]
    public class EventEntity : Entity
    {
        /// <summary>
        /// 获取指定事件实体类型的事件名称
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static string GetEventName<TEntity>() where TEntity : EventEntity
        {
            return GetEventName(typeof(TEntity));
        }

        /// <summary>
        /// 获取指定事件实体类型的事件名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetEventName(Type type)
        {
            Contract.Requires(type != null);

            EventAttribute attr = type.GetAttribute<EventAttribute>(true);
            if (attr == null || attr.Name == null)
                return "EVENT_" + type.FullName;

            return attr.Name;
        }
    }

    /// <summary>
    /// 用于标注事件实体
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EventAttribute : Attribute
    {
        public EventAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
