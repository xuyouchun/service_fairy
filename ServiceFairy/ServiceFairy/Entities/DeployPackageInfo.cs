using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common;
using Common.Utility;
using Common.Contracts;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 安装包信息
    /// </summary>
    [Serializable, DataContract]
    public class DeployPackageInfo
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        [DataMember]
        public int Size { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        [DataMember]
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// 安装包格式
        /// </summary>
        [DataMember]
        public DeployPackageFormat Format { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ID={1}", Title, Id);
        }
    }

    /// <summary>
    /// 服务的安装包信息
    /// </summary>
    [Serializable, DataContract]
    public class ServiceDeployPackageInfo : DeployPackageInfo
    {
        public ServiceDeployPackageInfo()
        {

        }

        /// <summary>
        /// 服务描述
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }

        public static readonly IEqualityComparer<ServiceDeployPackageInfo> Comparer = new ServiceDeployPackageInfoComparer();

        class ServiceDeployPackageInfoComparer : IEqualityComparer<ServiceDeployPackageInfo>
        {
            public bool Equals(ServiceDeployPackageInfo x, ServiceDeployPackageInfo y)
            {
                if (x == null || y == null)
                    return x == null && y == null;

                return x.Id == y.Id;
            }

            public int GetHashCode(ServiceDeployPackageInfo obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        public static ServiceDeployPackageInfo DeserailizeFromFile(string file)
        {
            return XmlUtility.DeserailizeFromFile<ServiceDeployPackageInfo>(file);
        }

        public static void SerializeToFile(ServiceDeployPackageInfo info, string file)
        {
            XmlUtility.SerializeToFile(info, file);
        }
    }

    /// <summary>
    /// Framework安装包信息
    /// </summary>
    [Serializable, DataContract]
    public class PlatformDeployPackageInfo : DeployPackageInfo
    {
        public static PlatformDeployPackageInfo DeserailizeFromFile(string file)
        {
            return XmlUtility.DeserailizeFromFile<PlatformDeployPackageInfo>(file);
        }

        public static void SerializeToFile(PlatformDeployPackageInfo info, string file)
        {
            XmlUtility.SerializeToFile(info, file);
        }
    }

    /// <summary>
    /// 安装包格式
    /// </summary>
    public enum DeployPackageFormat
    {
        /// <summary>
        /// GZip方式压缩
        /// </summary>
        [Desc("GZip压缩文件")]
        GZipCompress,
    }
}
