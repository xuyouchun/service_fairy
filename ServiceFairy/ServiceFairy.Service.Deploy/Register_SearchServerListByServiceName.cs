using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Framework.TrayPlatform;
using Common.Contracts.Service;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 通过名称获取服务器列表的请求
    /// </summary>
    [Serializable]
    public class Register_SearchServerListByServiceName_Request : RequestEntity
    {
        /// <summary>
        /// 服务信息
        /// </summary>
        public ServiceDesc ServiceDesc { get; set; }
    }

    /// <summary>
    /// 通过名称获取服务器列表的应答
    /// </summary>
    [Serializable]
    public class Register_SearchServerListByServiceName_Reply : ReplyEntity
    {
        /// <summary>
        /// 调用方式
        /// </summary>
        public AppInvokeInfo[] InvokeInfos { get; set; }
    }
}
