using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 将UserName批量转换为UserId－请求
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserIds_Request : RequestEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserNames)]
        public string[] UserNames { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserNames, "UserNames");
        }
    }

    /// <summary>
    /// 将UserName批量转换为UserId－应答
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserIds_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds)]
        public int[] UserIds { get; set; }
    }
}
