using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 执行命令－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_DoCommand_Request : RequestEntity
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Args { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Name, "Name");
        }
    }

    /// <summary>
    /// 执行命令－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_DoCommand_Reply : ReplyEntity
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Result { get; set; }
    }
}
