using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 保持用户的连接状态－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_KeepUserConnection_Request : UserCenterRequestEntity
    {
        /// <summary>
        /// 连接状态
        /// </summary>
        [DataMember]
        public UserConnectionInfo[] ConnectionInfos { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(ConnectionInfos, "ConnectionInfos");
        }
    }
}
