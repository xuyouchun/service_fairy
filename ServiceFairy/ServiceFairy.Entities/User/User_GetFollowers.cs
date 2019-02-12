using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 获取粉丝列表－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取粉丝列表－应答")]
    public class User_GetFollowers_Reply : ReplyEntity
    {
        /// <summary>
        /// 粉丝ID
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserIds)]
        public int[] UserIds { get; set; }
    }
}
