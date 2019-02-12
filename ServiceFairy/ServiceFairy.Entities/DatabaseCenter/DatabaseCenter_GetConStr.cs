using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.DatabaseCenter
{
    /// <summary>
    /// 获取数据库连接字符串－请求
    /// </summary>
    [Serializable, DataContract]
    public class DatabaseCenter_GetConStr_Request : RequestEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }

    /// <summary>
    /// 获取数据库连接字符串－应答
    /// </summary>
    [Serializable, DataContract]
    public class DatabaseCenter_GetConStr_Reply : ReplyEntity
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        [DataMember]
        public string ConStr { get; set; }
    }
}
