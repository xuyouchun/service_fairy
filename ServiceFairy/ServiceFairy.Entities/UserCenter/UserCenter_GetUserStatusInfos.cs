using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 获取用户的状态信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserStatusInfos_Request : RequestEntity
    {
        /// <summary>
        /// 用户集合
        /// </summary>
        [DataMember]
        public Users Users { get; set; }
    }

    /// <summary>
    /// 获取用户的状态信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserStatusInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 信息
        /// </summary>
        [DataMember]
        public UserStatusInfo[] Infos { get; set; }
    }
}
