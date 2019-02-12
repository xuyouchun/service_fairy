using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// AppService的存根数据
    /// </summary>
    [Serializable, DataContract]
    public class AppServiceCookie
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        [DataMember]
        public string DataType { get; set; }

        /// <summary>
        /// 所属服务的终端
        /// </summary>
        [DataMember]
        public ServiceEndPoint EndPoint { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public byte[] Data { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", DataType, EndPoint);
        }
    }

    /// <summary>
    /// AppService的Cookie集合
    /// </summary>
    [Serializable, DataContract]
    public class AppServiceCookieCollection
    {
        /// <summary>
        /// Cookie集合
        /// </summary>
        [DataMember]
        public AppServiceCookie[] Cookies { get; set; }
    }

    /// <summary>
    /// Cookie的读取器
    /// </summary>
    public class AppServiceCookieReader
    {
        public AppServiceCookieReader(AppServiceCookieCollection cookies)
        {
            Contract.Requires(cookies != null);

            _wrapper = new Lazy<Wrapper>(() => _Load(cookies));
        }

        private readonly Lazy<Wrapper> _wrapper;

        class Wrapper
        {
            public Dictionary<Guid, AppServiceCookie[]> DictOfClientId;
            public Dictionary<ServiceDesc, AppServiceCookie[]> DictOfServiceDesc;
            public Dictionary<ServiceEndPoint, AppServiceCookie[]> DictOfServiceEndPoint;
        }

        private Wrapper _Load(AppServiceCookieCollection cookies)
        {
            return new Wrapper() {
                DictOfClientId = cookies.Cookies.GroupBy(c => c.EndPoint.ClientId).ToDictionary(item => item.Key, item => item.ToArray()),
                DictOfServiceDesc = cookies.Cookies.GroupBy(c => c.EndPoint.ServiceDesc).ToDictionary(item => item.Key, item => item.ToArray()),
                DictOfServiceEndPoint = cookies.Cookies.GroupBy(c => c.EndPoint).ToDictionary(item => item.Key, item => item.ToArray()),
            };
        }
    }
}
