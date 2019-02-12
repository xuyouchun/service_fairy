﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 删除服务安装包－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_DeleteServiceDeployPackages_Request : RequestEntity
    {
        /// <summary>
        /// 安装包ID
        /// </summary>
        [DataMember]
        public Guid[] DeployIds { get; set; }
    }
}
