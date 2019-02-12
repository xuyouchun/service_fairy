using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 删除本地日志组－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_DeleteLocalLogGroups_Request : RequestEntity
    {
        /// <summary>
        /// 日志组
        /// </summary>
        [DataMember]
        public string[] Groups { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Groups, "Groups");
        }
    }
}
