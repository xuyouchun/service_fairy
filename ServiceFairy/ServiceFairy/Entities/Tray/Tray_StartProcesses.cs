using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 启动新进程－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_StartProcesses_Request : RequestEntity
    {
        /// <summary>
        /// 进程启动信息
        /// </summary>
        [DataMember]
        public ProcessStartInfo[] StartInfos { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(StartInfos, "StartInfos");
        }
    }

    /// <summary>
    /// 进程启动信息
    /// </summary>
    [Serializable, DataContract]
    public class ProcessStartInfo
    {
        /// <summary>
        /// 进程路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 进程参数
        /// </summary>
        [DataMember]
        public string Args { get; set; }
    }
}
