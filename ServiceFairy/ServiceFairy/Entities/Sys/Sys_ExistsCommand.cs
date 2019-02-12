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
    /// 判断指定的接口是否存在－请求
    /// </summary>
    [Serializable, DataContract]
    public class Sys_ExistsCommand_Request : RequestEntity
    {
        /// <summary>
        /// 接口
        /// </summary>
        [DataMember]
        public CommandDesc[] CommandDescs { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(CommandDescs, "CommandDescs");
        }
    }

    /// <summary>
    /// 判断指定的接口是否存在－应答
    /// </summary>
    [Serializable, DataContract]
    public class Sys_ExistsCommand_Reply : ReplyEntity
    {
        /// <summary>
        /// 存在的服务
        /// </summary>
        [DataMember]
        public CommandDesc[] CommandDescs { get; set; }
    }
}
