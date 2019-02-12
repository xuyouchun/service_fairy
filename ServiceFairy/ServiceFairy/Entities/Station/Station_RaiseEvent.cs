using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Station
{
    /// <summary>
    /// 激发事件－请求
    /// </summary>
    [Serializable, DataContract]
    public class Station_RaiseEvent_Request : ServiceRequestEntity
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        [DataMember]
        public string EventName { get; set; }

        /// <summary>
        /// 事件参数
        /// </summary>
        [DataMember]
        public byte[] EventArgs { get; set; }

        /// <summary>
        /// 是否通知其它Regsiter服务继续分发该事件
        /// </summary>
        [DataMember]
        public bool EnableRoute { get; set; }
    }
}
