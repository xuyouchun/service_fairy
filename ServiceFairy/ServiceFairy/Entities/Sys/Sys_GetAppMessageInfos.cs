using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 获取消息列表－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppMessageInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 信息
        /// </summary>
        [DataMember]
        public AppMessageInfo[] Infos { get; set; }
    }
}
