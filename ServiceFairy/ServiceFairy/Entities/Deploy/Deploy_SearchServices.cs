using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Deploy
{
    /// <summary>
    /// 寻找指定服务所在的位置－请求
    /// </summary>
    [Serializable, DataContract]
    public class Deploy_SearchServices_Request : RequestEntity
    {
        /// <summary>
        /// 服务描述，如果版本号为空，则搜索所有的服务
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }
    }

    /// <summary>
    /// 寻找指定服务所在的位置－应答
    /// </summary>
    [Serializable, DataContract]
    public class Deploy_SearchServices_Reply : ReplyEntity
    {
        /// <summary>
        /// 通信方式
        /// </summary>
        [DataMember]
        public AppInvokeInfo[] AppInvokeInfos { get; set; }
    }
}
