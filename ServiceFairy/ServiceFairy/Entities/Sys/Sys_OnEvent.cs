using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Entities.Sys
{
    /// <summary>
    /// 事件通知－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_OnEvent_Request : SysRequestEntity
    {
        /// <summary>
        /// 事件源的ClientID
        /// </summary>
        [DataMember]
        public ServiceEndPoint Source { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        [DataMember]
        public string EventName { get; set; }

        /// <summary>
        /// 事件参数
        /// </summary>
        [DataMember]
        public byte[] EventArgs { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (Source == null)
                throw new ServiceException(ServerErrorCode.ArgumentError, "事件源不允许为空");
        }
    }
}
