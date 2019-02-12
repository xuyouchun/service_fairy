using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 将用户ID转换为用户名－请求
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ConvertUserIdToNames_Request : RequestEntity
    {
        /// <summary>
        /// 用户
        /// </summary>
        [DataMember]
        public int[] UserIds { get; set; }

        /// <summary>
        /// 是否刷样报缓存
        /// </summary>
        [DataMember]
        public bool Refresh { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(UserIds, "UserIds");
        }
    }

    /// <summary>
    /// 将用户ID转换为用户名－应答
    /// </summary>
    [Serializable, DataContract]
    public class UserCenter_ConvertUserIdToNames_Reply : ReplyEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string[] UserNames { get; set; }
    }
}
