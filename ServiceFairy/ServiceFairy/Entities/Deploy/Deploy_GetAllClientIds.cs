using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Deploy
{
    /// <summary>
    /// 获取所有服务终端的唯一标识－应答
    /// </summary>
    [Serializable, DataContract]
    public class Deploy_GetAllClientIds_Reply : ReplyEntity
    {
        /// <summary>
        /// 服务终端标识
        /// </summary>
        [DataMember]
        public Guid[] ClientIds { get; set; }
    }
}
