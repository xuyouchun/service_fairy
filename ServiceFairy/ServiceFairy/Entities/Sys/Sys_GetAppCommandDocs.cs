using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 获取接口文档－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppCommandDocs_Request : RequestEntity
    {
        /// <summary>
        /// 接口
        /// </summary>
        [DataMember]
        public CommandDesc[] CommandDescs { get; set; }

        /// <summary>
        /// 格式，如果为空则返回所有格式
        /// </summary>
        [DataMember]
        public DataFormat[] Formats { get; set; }
    }

    /// <summary>
    /// 获取接口文档－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppCommandDocs_Reply : ReplyEntity
    {
        /// <summary>
        /// 参数
        /// </summary>
        [DataMember]
        public AppCommandDoc[] Docs { get; set; }
    }

    /// <summary>
    /// 接口参数
    /// </summary>
    [Serializable, DataContract]
    public class AppCommandDoc
    {
        /// <summary>
        /// 接口
        /// </summary>
        [DataMember]
        public CommandDesc CommandDesc { get; set; }

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
        /// 安全状态文档
        /// </summary>
        [DataMember]
        public SecurityDoc SecurityDoc { get; set; }

        /// <summary>
        /// 可用状态文档
        /// </summary>
        [DataMember]
        public UsableDoc UsableDoc { get; set; }

        /// <summary>
        /// 输入参数的示例及文档
        /// </summary>
        [DataMember]
        public AppArgumentData Input { get; set; }

        /// <summary>
        /// 输出参数的示例及文档
        /// </summary>
        [DataMember]
        public AppArgumentData Output { get; set; }
    }

    /// <summary>
    /// 安全级别描述
    /// </summary>
    [Serializable, DataContract]
    public class SecurityDoc
    {
        /// <summary>
        /// 级别
        /// </summary>
        [DataMember]
        public int Level { get; set; }

        /// <summary>
        /// 级别描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }
    }

    /// <summary>
    /// 可用状态文本
    /// </summary>
    [Serializable, DataContract]
    public class UsableDoc
    {
        /// <summary>
        /// 可用状态
        /// </summary>
        [DataMember]
        public UsableType Usable { get; set; }

        /// <summary>
        /// 可用状态描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }
    }

    /// <summary>
    /// 参数的示例及文档
    /// </summary>
    [Serializable, DataContract]
    public class AppArgumentData
    {
        /// <summary>
        /// 是否为空
        /// </summary>
        [DataMember]
        public bool IsEmpty { get; set; }

        /// <summary>
        /// 示例
        /// </summary>
        [DataMember]
        public AppArgumentDataSample[] Samples { get; set; }

        /// <summary>
        /// 文档
        /// </summary>
        [DataMember]
        public TypeDocTree DocTree { get; set; }
    }

    /// <summary>
    /// 参数的示例
    /// </summary>
    [Serializable, DataContract]
    public class AppArgumentDataSample
    {
        /// <summary>
        /// 格式
        /// </summary>
        [DataMember]
        public DataFormat Format { get; set; }

        /// <summary>
        /// 示例
        /// </summary>
        [DataMember]
        public string Sample { get; set; }
    }
}
