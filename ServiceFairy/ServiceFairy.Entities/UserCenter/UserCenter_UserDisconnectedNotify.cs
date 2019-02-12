using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 用户连接断开通知－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_UserDisconnectedNotify_Request : RequestEntity
    {
        /// <summary>
        /// 连接状态
        /// </summary>
        [DataMember]
        public UserDisconnectedInfo[] DisconnectionInfos { get; set; }

        /// <summary>
        /// 是否允许路由
        /// </summary>
        [DataMember]
        public bool EnableRoute { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(DisconnectionInfos, "DisconnectionInfos");
        }
    }
}
