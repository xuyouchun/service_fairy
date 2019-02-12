using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取系统环境变量－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetSystemEnvironmentVariables_Request : RequestEntity
    {
        /// <summary>
        /// 环境变量名称，如果留空则返回全部环境变量
        /// </summary>
        [DataMember]
        public string[] Names { get; set; }
    }

    /// <summary>
    /// 系统环境变量
    /// </summary>
    [Serializable, DataContract]
    public class SystemEnvironmentVariable
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }
    }

    /// <summary>
    /// 获取系统环境变量－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetSystemEnvironmentVariables_Reply : ReplyEntity
    {
        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public SystemEnvironmentVariable[] Variables { get; set; }
    }
}
