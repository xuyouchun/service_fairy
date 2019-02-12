using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 删除联系人－请求
    /// </summary>
    [Serializable, DataContract, Summary("删除联系人－请求")]
    public class User_RemoveContact_Request : RequestEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserName)]
        public string UserName { get; set; }
    }
}
