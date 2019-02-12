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
    /// 获取服务的文件集合－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_GetAppServiceFileInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        [DataMember]
        public AppFileInfo[] FileInfos { get; set; }
    }
}
