using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取文件系统顶级目录信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_FsGetRootDirectoryInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 目录信息
        /// </summary>
        [DataMember]
        public FsDirectoryInfo[] Infos { get; set; }
    }
}
