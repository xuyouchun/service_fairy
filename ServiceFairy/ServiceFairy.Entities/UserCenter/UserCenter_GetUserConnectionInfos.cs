using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 获取用户的连接状态
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserConnectionInfos_Request : UserCenterRequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserIds, "UserIds");
        }
    }

    /// <summary>
    /// 获取用户的连接状态
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_GetUserConnectionInfos_Reply : UserCenterReplyEntity
    {
        /// <summary>
        /// 连接信息
        /// </summary>
        [DataMember]
        public UserConnectionInfo[] Infos { get; set; }
    }
}
