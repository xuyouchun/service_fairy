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
    /// 获取安装包列表，请求
    /// </summary>
    [Serializable, DataContract]
    public class Deploy_GetPackageList_Request : RequestEntity
    {
        /// <summary>
        /// 安装包名称（服务名称），如果为空，则获取全部的安装包列表
        /// </summary>
        [DataMember]
        public string[] ServiceNames { get; set; }
    }

    /// <summary>
    /// 获取安装包列表，应答
    /// </summary>
    [Serializable, DataContract]
    public class Deploy_GetPackageList_Reply : ReplyEntity
    {
        /// <summary>
        /// 安装包的服务描述列表
        /// </summary>
        [DataMember]
        public ServiceDesc[] ServiceDescs { get; set; }
    }
}
