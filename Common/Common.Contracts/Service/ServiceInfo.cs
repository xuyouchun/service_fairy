using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.Net;
using Common.Contracts.Service;
using Common.Utility;
using System.Collections;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 服务描述
    /// </summary>
    [Serializable, DataContract]
    public class ServiceInfo
    {
        public ServiceInfo()
        {
            
        }

        /// <summary>
        /// 服务的描述
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public AppServiceStatus Status { get; set; }

        /// <summary>
        /// 启动时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

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
        /// 部署版本标识
        /// </summary>
        [DataMember]
        public Guid DeployId { get; set; }

        /// <summary>
        /// 是否为有效状态
        /// </summary>
        [DataMember]
        public bool Avaliable { get; set; }

        /// <summary>
        /// 运行时信息
        /// </summary>
        [DataMember]
        public AppServiceRuntimeInfo RuntimeInfo { get; set; }

        public override int GetHashCode()
        {
            return ServiceDesc.GetHashCode() ^ Status.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ServiceInfo))
                return false;

            ServiceInfo sInfo = (ServiceInfo)obj;
            return ServiceDesc == sInfo.ServiceDesc && Status == sInfo.Status;
        }

        public static bool operator ==(ServiceInfo obj1, ServiceInfo obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(ServiceInfo obj1, ServiceInfo obj2)
        {
            return !object.Equals(obj1, obj2);
        }

        public override string ToString()
        {
            return ServiceDesc + " " + Status.GetDesc();
        }

        public static readonly IEqualityComparer<ServiceInfo> Comparer = new ServiceInfoEqualityComparer();

        #region Class ServiceInfoEqualityComparer ...

        private class ServiceInfoEqualityComparer : IEqualityComparer<ServiceInfo>
        {
            public bool Equals(ServiceInfo x, ServiceInfo y)
            {
                if (x == null || y == null)
                    return x == null && y == null;

                return object.Equals(x.ServiceDesc, y.ServiceDesc);       
            }

            public int GetHashCode(ServiceInfo obj)
            {
                return obj == null || obj.ServiceDesc == null ? 0 : obj.ServiceDesc.GetHashCode();
            }
        }

        #endregion
    }

    /// <summary>
    /// 服务的运行时信息
    /// </summary>
    [Serializable, DataContract]
    public class AppServiceRuntimeInfo
    {
        /// <summary>
        /// 内存占用
        /// </summary>
        [DataMember]
        public long Memory { get; set; }
    }
}
