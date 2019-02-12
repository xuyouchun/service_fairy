using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 获取状态码描述信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppStatusCodeInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 状态码描述信息
        /// </summary>
        [DataMember]
        public AppStatusCodeInfo[] Infos { get; set; }
    }
}
