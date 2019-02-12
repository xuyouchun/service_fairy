using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 一次上传全部文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_UploadAll_Request : RequestEntity
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 文件缓冲区
        /// </summary>
        [DataMember]
        public byte[] Buffer { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(Path, "Path");
            EntityValidate.ValidateNull(Buffer, "Buffer");
        }
    }
}
