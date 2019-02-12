using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 将用户名转换为用户ID－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ConvertUserNameToIds_Request : RequestEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string[] UserNames { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserNames, "UserNames");
        }
    }

    /// <summary>
    /// 将用户名转换为用户ID－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ConvertUserNameToIds_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }
}
