using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 解析为用户名－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ParseToUserNames_Request : RequestEntity
    {
        /// <summary>
        /// 用户
        /// </summary>
        [DataMember]
        public Users Users { get; set; }
    }

    /// <summary>
    /// 解析为用户名－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ParseToUserNames_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string[] UserNames { get; set; }
    }
}
