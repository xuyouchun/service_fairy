using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 根据UserId获取UserName－请求
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserNames_Request : RequestEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds)]
        public int[] UserIds { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserIds, "UserIds");
        }
    }

    /// <summary>
    /// 根据UserId获取UserName－应答
    /// </summary>
    [Serializable, DataContract]
    public class User_GetUserNames_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserNames), Remarks("按输入UserId的顺序排列，如果该UserId无效，则对应的项为空")]
        public string[] UserNames { get; set; }
    }
}
