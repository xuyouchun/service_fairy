using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 结束进程－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_KillProcesses_Request : RequestEntity
    {
        /// <summary>
        /// 进程ID
        /// </summary>
        [DataMember]
        public int[] ProcessIds { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(ProcessIds, "ProcessIds");
        }
    }
}
