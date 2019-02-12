using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Email
{
    /// <summary>
    /// 发送邮件－请求
    /// </summary>
    [Serializable, DataContract]
    public class Email_Send_Request : RequestEntity
    {
        /// <summary>
        /// 邮件
        /// </summary>
        [DataMember]
        public EmailItem[] Emails { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Emails, "Emails");
        }
    }
}
