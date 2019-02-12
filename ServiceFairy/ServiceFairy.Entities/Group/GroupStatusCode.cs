using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 群组接口状态码
    /// </summary>
    public enum GroupStatusCode : int
    {
        /// <summary>
        /// 错误
        /// </summary>
        [Desc("Error")]
        Error = SFStatusCodes.Group,

        /// <summary>
        /// 群组不存在
        /// </summary>
        [Desc("群组不存在")]
        InvalidGroup = Error | 1,

        /// <summary>
        /// 用户不属于该群组
        /// </summary>
        [Desc("用户不属于该群组")]
        NotMemberOfGroup = Error | 2,

        /// <summary>
        /// 用户属于该群组
        /// </summary>
        [Desc("用户属于该群组")]
        MemberOfGroup = Error | 3,

        /// <summary>
        /// 用户不是该群组的创建者
        /// </summary>
        [Desc("用户不是该群组的创建者")]
        NotCreatorOfGroup = Error | 4,

        /// <summary>
        /// 用户是该群组的创建者
        /// </summary>
        [Desc("用户是该群组的创建者")]
        CreatorOfGroup = Error | 5,

        /// <summary>
        /// 群组成员为空
        /// </summary>
        [Desc("群组成员为空")]
        NoMembers = Error | 6,
    }
}
