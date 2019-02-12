using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ServiceFairy.Entities.Message
{
    /// <summary>
    /// 批量删除消息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Message_RemoveMessages_Request : RequestEntity
    {
        /// <summary>
        /// 消息索引号
        /// </summary>
        [DataMember]
        public long[] MsgIndexes { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(MsgIndexes, "MsgIndexes");
        }
    }
}
