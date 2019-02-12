using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 用户上线
    /// </summary>
    [Serializable, DataContract, Event(ServiceEventNames.EVENT_USERCENTER_ONLINE)]
    public class UserCenter_Online_Event : EventEntity
    {
        /// <summary>
        /// 上线用户的ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }

    /// <summary>
    /// 用户离线
    /// </summary>
    [Serializable, DataContract, Event(ServiceEventNames.EVENT_USERCENTER_OFFLINE)]
    public class UserCenter_Offline_Event : EventEntity
    {
        /// <summary>
        /// 上线用户的ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }
    }
}
