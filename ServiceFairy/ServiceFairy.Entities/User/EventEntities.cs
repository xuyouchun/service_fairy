using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 用户注册事件
    /// </summary>
    [Serializable, DataContract, Event(ServiceEventNames.EVENT_USER_REGISTER)]
    public class User_Register_Event : EventEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }

    /// <summary>
    /// 用户登录事件
    /// </summary>
    [Serializable, DataContract, Event(ServiceEventNames.EVENT_USER_LOGIN)]
    public class User_Login_Event : EventEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }

    /// <summary>
    /// 联系人列表发生变化
    /// </summary>
    [Serializable, DataContract, Event(ServiceEventNames.EVENT_USER_RELATION_CHANGED)]
    public class User_RelationChanged_Event : EventEntity
    {
        /// <summary>
        /// 发生变化的UserId
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }

    /// <summary>
    /// 用户状态发生变化
    /// </summary>
    [Serializable, DataContract, Event(ServiceEventNames.EVENT_USER_STATUS_CHANGED)]
    public class User_StatusChanged_Event : EventEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }

    /// <summary>
    /// 用户信息变化
    /// </summary>
    [Serializable, DataContract, Event(ServiceEventNames.EVENT_USER_INFO_CHANGED)]
    public class User_InfoChanged_Event : EventEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }
}
