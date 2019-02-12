using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 系统日志的分组
    /// </summary>
    [Serializable, DataContract]
    public class SystemLogGroup
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 日志数量
        /// </summary>
        [DataMember]
        public int Count { get; set; }
    }

    /// <summary>
    /// 获取系统日志的分组－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetSystemLogGroups_Reply : ReplyEntity
    {
        /// <summary>
        /// 分组
        /// </summary>
        [DataMember]
        public SystemLogGroup[] Groups { get; set; }
    }
}
