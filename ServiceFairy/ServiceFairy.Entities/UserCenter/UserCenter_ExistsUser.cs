using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 判断指定的用户是否在该用户中心上注册－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ExistsUser_Request : RequestEntity
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
    /// 判断指定的用户是否在该用户中心上注册－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ExistsUser_Reply : ReplyEntity
    {
        /// <summary>
        /// 在线的用户ID
        /// </summary>
        [DataMember]
        public int[] ExistsUserIds { get; set; }
    }
}
