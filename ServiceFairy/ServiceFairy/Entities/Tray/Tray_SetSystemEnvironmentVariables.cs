using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 设置系统环境变量－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_SetSystemEnvironmentVariables_Request : RequestEntity
    {
        /// <summary>
        /// 环境变量
        /// </summary>
        [DataMember]
        public SystemEnvironmentVariable[] Variables { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Variables, "Variables");
        }
    }
}
