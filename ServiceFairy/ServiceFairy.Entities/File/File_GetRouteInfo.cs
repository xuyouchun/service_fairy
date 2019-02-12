using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 为路径寻找终端－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_GetRouteInfo_Request : RequestEntity
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string[] Paths { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Paths, "Paths");
        }
    }

    /// <summary>
    /// 为路径寻找终端－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_GetRouteInfo_Reply : ReplyEntity
    {
        /// <summary>
        /// 路由信息
        /// </summary>
        [DataMember]
        public FileRouteInfo[] Infos { get; set; }
    }

    /// <summary>
    /// 路由信息
    /// </summary>
    [Serializable, DataContract]
    public class FileRouteInfo
    {
        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 终端ID
        /// </summary>
        [DataMember]
        public Guid ClientID { get; set; }
    }
}
