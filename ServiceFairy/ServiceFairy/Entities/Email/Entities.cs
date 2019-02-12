using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Email
{
    /// <summary>
    /// 邮件
    /// </summary>
    [Serializable, DataContract]
    public class EmailItem
    {
        /// <summary>
        /// 发送者
        /// </summary>
        [DataMember]
        public string From { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        [DataMember]
        public string[] To { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        [DataMember]
        public string[] Cc { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 标题编码
        /// </summary>
        [DataMember]
        public string TitleEncoding { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Body { get; set; }

        /// <summary>
        /// 内容编码
        /// </summary>
        [DataMember]
        public string BodyEncoding { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [DataMember]
        public DateTime SendTime { get; set; }
    }
}
