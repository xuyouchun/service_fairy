using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 用户请求实体基类
    /// </summary>
    [Serializable, DataContract]
    public class UserRequestEntity : RequestEntity
    {
        
    }

    /// <summary>
    /// 用户应答实体基类
    /// </summary>
    [Serializable, DataContract]
    public class UserReplyEntity : ReplyEntity
    {

    }
}
