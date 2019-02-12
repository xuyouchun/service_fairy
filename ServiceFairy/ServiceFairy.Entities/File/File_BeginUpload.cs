using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 开始上传文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_BeginUpload_Request : RequestEntity
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(Path, "Path");
        }
    }

    /// <summary>
    /// 开始上传文件－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_BeginUpload_Reply : ReplyEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }
    }
}
