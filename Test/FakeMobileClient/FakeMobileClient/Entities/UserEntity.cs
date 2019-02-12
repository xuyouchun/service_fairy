using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FakeMobileClient.Entities
{
    /// <summary>
    /// 用户请求实体基类
    /// </summary>
    [Serializable, DataContract]
    public class UserRequestEntity : RequestEntity
    {
        /// <summary>
        /// 用户会话唯一标识
        /// </summary>
        [DataMember]
        public string SeccionID { get; set; }

        /// <summary>
        /// 安全码，未登录时没有此安全码
        /// </summary>
        [DataMember]
        public string SecurityCode { get; set; }
    }

    /// <summary>
    /// 用户应答实体基类
    /// </summary>
    [Serializable, DataContract]
    public class UserReplyEntity : ReplyEntity
    {

    }
}
