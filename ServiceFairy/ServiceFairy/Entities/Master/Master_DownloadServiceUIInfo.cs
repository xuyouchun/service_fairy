using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 下载服务管理器的信息－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_DownloadServiceUIInfo_Request : RequestEntity
    {
        /// <summary>
        /// 服务描述
        /// </summary>
        [DataMember]
        public ServiceDesc[] ServiceDescs { get; set; }

        /// <summary>
        /// 是否下载程序集
        /// </summary>
        [DataMember]
        public bool IncludeAssembly { get; set; }

        /// <summary>
        /// 是否下载图标等资源
        /// </summary>
        [DataMember]
        public bool IncludeResource { get; set; }
    }

    /// <summary>
    /// 下载服务管理器的信息－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_DownloadServiceUIInfo_Reply : ReplyEntity
    {
        /// <summary>
        /// 服务管理器信息
        /// </summary>
        [DataMember]
        public ServiceUIInfo[] ServiceUIInfos { get; set; }
    }

    /// <summary>
    /// 服务管理器信息
    /// </summary>
    [Serializable, DataContract]
    public class ServiceUIInfo
    {
        /// <summary>
        /// 服务描述
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }

        /// <summary>
        /// 权重（按此排序）
        /// </summary>
        [DataMember]
        public int Weight { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [DataMember]
        public AppServiceCategory Category { get; set; }

        /// <summary>
        /// 主程序集
        /// </summary>
        [DataMember]
        public byte[] MainAssembly { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [DataMember]
        public byte[] Icon { get; set; }

        /// <summary>
        /// 配置文件
        /// </summary>
        [DataMember]
        public string AppConfig { get; set; }

        public override string ToString()
        {
            return ServiceDesc.ToString();
        }

        public static ServiceUIInfo Clone(ServiceUIInfo prototype, bool includeAssembly, bool includeResource)
        {
            Contract.Requires(prototype != null);

            return new ServiceUIInfo() {
                ServiceDesc = prototype.ServiceDesc,
                Icon = includeResource ? prototype.Icon : null,
                AppConfig = prototype.AppConfig,
                Category = prototype.Category,
                Title = prototype.Title,
                Desc = prototype.Desc,
                Weight = prototype.Weight,
                MainAssembly = includeAssembly ? prototype.MainAssembly : null,
            };
        }
    }
}
