using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ServiceFairy.Entities;

namespace BhFairy.Entities
{
    /// <summary>
    /// 请求实体
    /// </summary>
    [Serializable, DataContract]
    public class BhReuqestEntity : RequestEntity
    {

    }

    /// <summary>
    /// 应答实体
    /// </summary>
    [Serializable, DataContract]
    public class BhReplyEntity : ReplyEntity
    {

    }
}
