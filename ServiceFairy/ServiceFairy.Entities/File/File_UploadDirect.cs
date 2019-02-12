using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 直接上传文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_UploadDirect_Request : RequestEntity
    {
        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 是否已经上传完毕
        /// </summary>
        [DataMember]
        public bool AtEnd { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(Path, "Path");
            EntityValidate.ValidateNull(Buffer, "Buffer");
        }
    }

    /// <summary>
    /// 直接上传文件－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_UploadDirect_Reply : ReplyEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }
    }
}
