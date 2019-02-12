using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 删除文件－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_FsDeletePath_Request : RequestEntity
    {
        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string[] Paths { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Paths);
        }
    }
}
