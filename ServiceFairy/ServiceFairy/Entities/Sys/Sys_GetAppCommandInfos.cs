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
    /// 获取所有接口－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppCommandInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 所有AppCommand信息
        /// </summary>
        [DataMember]
        public AppCommandInfo[] AppCommandInfos { get; set; }
    }
}
