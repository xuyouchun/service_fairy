using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FakeMobileClient.Entities
{
    /// <summary>
    /// 实体的基类
    /// </summary>
    [Serializable, DataContract]
    public class Entity
    {
        
    }

    /// <summary>
    /// 请求实体的基类
    /// </summary>
    [Serializable, DataContract]
    public class RequestEntity : Entity
    {

    }

    /// <summary>
    /// 应答实体的基类
    /// </summary>
    [Serializable, DataContract]
    public class ReplyEntity : Entity
    {

    }
}
