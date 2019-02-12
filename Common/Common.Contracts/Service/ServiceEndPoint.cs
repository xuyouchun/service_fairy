using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 服务终端
    /// </summary>
    [Serializable, DataContract]
    public class ServiceEndPoint
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="serviceDesc"></param>
        [JsonConstructor]
        public ServiceEndPoint(Guid clientId, ServiceDesc serviceDesc = null)
        {
            ClientId = clientId;
            ServiceDesc = serviceDesc;
        }

        /// <summary>
        /// 服务终端唯一标识
        /// </summary>
        [DataMember]
        public Guid ClientId { get; private set; }

        /// <summary>
        /// 终端服务描述
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; private set; }

        public override int GetHashCode()
        {
            return ClientId.GetHashCode() ^ (ServiceDesc == null ? 0 : ServiceDesc.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ServiceEndPoint))
                return false;

            ServiceEndPoint sep = (ServiceEndPoint)obj;
            return ClientId == sep.ClientId && ServiceDesc == sep.ServiceDesc;
        }

        public override string ToString()
        {
            if (ServiceDesc == null)
                return ClientId.ToString();

            return ClientId + "/" + ServiceDesc;
        }

        public static bool operator ==(ServiceEndPoint sep1, ServiceEndPoint sep2)
        {
            return object.Equals(sep1, sep2);
        }

        public static bool operator !=(ServiceEndPoint sep1, ServiceEndPoint sep2)
        {
            return !object.Equals(sep1, sep2);
        }

        /// <summary>
        /// 将字符串转换为ServiceEndPoint实体
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ServiceEndPoint Parse(string s)
        {
            Contract.Requires(s != null);

            ServiceEndPoint e;
            if (!TryParse(s, out e))
                throw new FormatException("ServiceEndPoint字符串格式错误");

            return e;
        }

        /// <summary>
        /// 将字符串转换为ServiceEndPoint实体
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out ServiceEndPoint value)
        {
            if (s == null)
                goto _error;

            int index = s.IndexOf('/');
            Guid clientId;
            ServiceDesc sd;

            if (index < 0)
            {
                if (!Guid.TryParse(s, out clientId))
                    goto _error;

                sd = null;
            }
            else
            {
                if (!Guid.TryParse(s.Substring(0, index), out clientId))
                    goto _error;

                sd = ServiceDesc.Parse(s.Substring(index + 1));
            }

            value = new ServiceEndPoint(clientId, sd);
            return true;

_error:
            value = null;
            return false;
        }
    }
}
