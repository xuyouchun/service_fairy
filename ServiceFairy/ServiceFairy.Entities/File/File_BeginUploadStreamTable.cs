using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Package.Storage;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 开始上传StreamTable－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_BeginUploadStreamTable_Request : RequestEntity
    {
        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 表信息，如果该值不为空，则创建新表
        /// </summary>
        [DataMember]
        public NewStreamTableInfo TableInfo { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(Path, "Path");
        }
    }

    /// <summary>
    /// 开始上传StreamTable－应答
    /// </summary>
    [Serializable, DataContract]
    public class File_BeginUploadStreamTable_Reply : ReplyEntity
    {
        /// <summary>
        /// 事务标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }
    }
}
