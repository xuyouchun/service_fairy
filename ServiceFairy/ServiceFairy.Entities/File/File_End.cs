using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 结束下载－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_End_Request : RequestEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string[] Tokens { get; set; }
    }
}
