using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Group
{
    /// <summary>
    /// 群组信息变化事件
    /// </summary>
    [Serializable, DataContract, Event(ServiceEventNames.EVENT_GROUP_INFO_CHANGED)]
    public class Group_InfoChanged_Event : EventEntity
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        [DataMember]
        public int[] GroupIds { get; set; }

        /// <summary>
        /// 受影响的用户
        /// </summary>
        [DataMember]
        public int[] EffectUserIds { get; set; }
    }
}
