using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取自定义命令列表－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetCustomCommandInfos_Reply : ReplyEntity
    {
        /// <summary>
        /// 自定义命令信息
        /// </summary>
        [DataMember]
        public CustomCommandInfo[] Infos { get; set; }
    }
}
