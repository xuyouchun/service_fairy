using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Package.Serializer;
using Common;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 消息
    /// </summary>
    [Serializable, DataContract]
    public class Msg
    {
        /// <summary>
        /// 消息ID，唯一标识
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        [DataMember]
        public int From { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        [DataMember]
        public string Method { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public byte[] Data { get; set; }

        /// <summary>
        /// 数据格式
        /// </summary>
        [DataMember]
        public DataFormat Format { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 消息属性
        /// </summary>
        [DataMember]
        public MsgProperty Property { get; set; }

        /// <summary>
        /// 通过实体类创建
        /// </summary>
        /// <param name="entity">消息实体对象</param>
        /// <param name="from">发送者</param>
        /// <param name="method">消息方法</param>
        /// <param name="format">消息编码格式</param>
        /// <param name="property">消息属性</param>
        /// <returns></returns>
        public static Msg Create(object entity, int from, string method = null, DataFormat format = DataFormat.Json, MsgProperty property = MsgProperty.Default)
        {
            byte[] bytes = (entity == null) ? null : SerializerUtility.SerializeToBytes(format, entity);

            Msg msg = new Msg() {
                Data = bytes, Format = format, ID = Guid.NewGuid(), From = from,
                Method = method ?? _GetDefaultMethod(entity), Property = property, Time = DateTime.UtcNow,
            };

            return msg;
        }

        private static string _GetDefaultMethod(object entity)
        {
            if (entity == null)
                return null;

            Type type = entity.GetType();
            return _methodDict.GetOrSet(entity.GetType(), MessageAttribute.GetMethod);
        }

        private static readonly Dictionary<Type, string> _methodDict = new Dictionary<Type, string>();
    }

    /// <summary>
    /// 用于标注消息实体的特征
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class MessageAttribute : Attribute, IDocSummaryProvider, IDocRemarksProvider
    {
        public MessageAttribute(string method, string title, string desc)
        {
            Contract.Requires(method != null);

            MethodParser mp = new MethodParser(method);
            SVersion v;
            ServiceDesc = mp.ServiceDesc.Version.IsEmpty ? new ServiceDesc(mp.ServiceDesc.Name, SVersion.Version_1) : mp.ServiceDesc;
            MessageDesc = new MessageDesc(mp.CommandDesc.Name, (v = mp.CommandDesc.Version).IsEmpty ? SVersion.Version_1 : v);
            Method = mp.ToString();

            Title = title ?? string.Empty;
            Desc = desc ?? string.Empty;
        }

        public MessageAttribute(string method, string title)
            : this(method, title, "")
        {

        }

        public MessageAttribute(string method)
            : this(method, "", "")
        {

        }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// 所属服务
        /// </summary>
        public ServiceDesc ServiceDesc { get; private set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        public MessageDesc MessageDesc { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; private set; }

        /// <summary>
        /// 从实体类型获取Method
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static string GetMethod(Type entityType)
        {
            MessageAttribute attr = entityType.GetAttribute<MessageAttribute>(true);
            string method = (attr == null) ? null : attr.Method;
            return method ?? (entityType.Name.TrimEnd("_Message"));
        }

        /// <summary>
        /// 转换为消息信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public AppMessageInfo ToAppMessageInfo(Type type)
        {
            return new AppMessageInfo(ServiceDesc, MessageDesc, new AppParameter(type), Title, Desc);
        }

        string IDocSummaryProvider.GetSummary()
        {
            return Title;
        }

        string IDocRemarksProvider.GetRemarks()
        {
            return Desc;
        }

        int IDocProvider.GetRank()
        {
            return 0;
        }
    }

    /// <summary>
    /// 消息的属性
    /// </summary>
    [Flags]
    public enum MsgProperty
    {
        Default = 0,

        /// <summary>
        /// 不可靠消息，丢失不会造成严重后果
        /// </summary>
        NotReliable = 0x01,

        /// <summary>
        /// 只有最后一条消息有意义（例如状态变化消息）
        /// </summary>
        Override = 0x02,

        /// <summary>
        /// 只发送给在线用户
        /// </summary>
        OnlyOnline = 0x04,
    }

    /// <summary>
    /// 消息池类型
    /// </summary>
    public enum MsgPoolType : byte
    {
        /// <summary>
        /// 用户消息池
        /// </summary>
        [Desc("用户消息池")]
        User = 0,

        /// <summary>
        /// 关注消息池
        /// </summary>
        [Desc("关注消息池")]
        Following = 1,

        /// <summary>
        /// 群组消息池
        /// </summary>
        [Desc("群组消息池")]
        Group = 2,
    }

    /// <summary>
    /// 消息池工具
    /// </summary>
    public static class MsgPool
    {
        /// <summary>
        /// 创建消息池Id
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static long CreatePoolId(MsgPoolType type, int id)
        {
            return (((long)type) << 56) + (int)id;
        }
    }
}
