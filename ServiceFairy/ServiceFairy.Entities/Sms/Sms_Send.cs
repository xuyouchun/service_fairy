using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Sms
{
    [Serializable, DataContract]
    public class Sms_Send_Request : RequestEntity
    {
        [DataMember]
        public string[] PhoneNumbers { get; set; }

        [DataMember]
        public string Content { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrEmpty(PhoneNumbers);
            EntityValidate.ValidateNullOrEmpty(Content);
        }
    }

    [Serializable, DataContract]
    public class Sms_Send_Reply : ReplyEntity
    {
        [DataMember]
        public int ResultCode { get; set; }

        [DataMember]
        public string ResultMsg { get; set; }
    }
}
