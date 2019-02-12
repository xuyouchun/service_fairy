using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 获取消息参数示例及文档－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppMessageDocs_Request : RequestEntity
    {
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public MessageDesc[] MessageDescs { get; set; }

        /// <summary>
        /// 格式，如果为空则返回所有格式
        /// </summary>
        [DataMember]
        public DataFormat[] Formats { get; set; }
    }

    /// <summary>
    /// 消息文档
    /// </summary>
    [Serializable, DataContract]
    public class AppMessageDoc
    {
        /// <summary>
        /// 消息描述
        /// </summary>
        [DataMember]
        public MessageDesc MessageDesc { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [DataMember]
        public string Summary { get; set; }

        /// <summary>
        /// 详述
        /// </summary>
        [DataMember]
        public string Remarks { get; set; }

        /// <summary>
        /// 数据的文档
        /// </summary>
        [DataMember]
        public AppArgumentData Data { get; set; }
    }

    /// <summary>
    /// 获取消息参数示例及文档－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppMessageDocs_Reply : ReplyEntity
    {
        /// <summary>
        /// 文档
        /// </summary>
        [DataMember]
        public AppMessageDoc[] Docs { get; set; }
    }
}
