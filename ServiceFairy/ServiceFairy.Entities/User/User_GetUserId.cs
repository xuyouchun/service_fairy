using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 将UserName转换为UserId－请求
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserId_Request : RequestEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserName)]
        public string UserName { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserName, "UserName");
        }
    }

    /// <summary>
    /// 将UserName转换为UserId－应答
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserId_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserId)]
        public int UserId { get; set; }
    }
}
