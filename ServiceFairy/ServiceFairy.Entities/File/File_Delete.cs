using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 删除文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class File_Delete_Request : RequestEntity
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public string[] Paths { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Paths, "Paths");
        }
    }
}
