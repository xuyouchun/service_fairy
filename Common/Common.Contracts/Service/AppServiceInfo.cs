using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Common.Contracts.Service
{
    /// <summary>
    /// AppService信息
    /// </summary>
    [Serializable, DataContract]
    public class AppServiceInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceDesc">服务的描述</param>
        /// <param name="title">标题</param>
        /// <param name="desc">描述</param>
        /// <param name="defaultBufferType"></param>
        /// <param name="defaultDataFormat"></param>
        public AppServiceInfo(ServiceDesc serviceDesc, string title, string desc, DataFormat defaultDataFormat, int weight, AppServiceCategory category)
        {
            Contract.Requires(serviceDesc != null);

            ServiceDesc = serviceDesc;
            Title = title;
            Desc = desc;
            DefaultDataFormat = defaultDataFormat;
            Weight = weight;
            Category = category;
        }

        public AppServiceInfo()
        {

        }

        /// <summary>
        /// 服务的描述
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; private set; }

        /// <summary>
        /// 权重
        /// </summary>
        [DataMember]
        public int Weight { get; private set; }

        /// <summary>
        /// 类别
        /// </summary>
        [DataMember]
        public AppServiceCategory Category { get; private set; }

        /// <summary>
        /// 默认编码方式
        /// </summary>
        [DataMember]
        public DataFormat DefaultDataFormat { get; private set; }

        public override string ToString()
        {
            return ServiceDesc.ToString();
        }
    }
}
