using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 获取联系人列表－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取联系人列表－应答")]
    public class User_GetContactList_Reply : ReplyEntity
    {
        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIdName)]
        public UserIdName[] Users { get; set; }
    }
}
