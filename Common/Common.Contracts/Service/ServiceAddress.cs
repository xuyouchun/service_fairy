using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Net;

namespace Common.Contracts.Service
{
    [Serializable, DataContract]
    public class ServiceAddress
    {
        [JsonConstructor]
        public ServiceAddress(string address, int port)
        {
            Address = address;
            Port = port;
        }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public string Address { get; private set; }

        /// <summary>
        /// 端口号
        /// </summary>
        [DataMember]
        public int Port { get; private set; }

        public override int GetHashCode()
        {
            return (Address ?? string.Empty).GetHashCode() ^ Port;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ServiceAddress))
                return false;

            ServiceAddress obj2 = (ServiceAddress)obj;
            return string.Equals(Address, obj2.Address, StringComparison.OrdinalIgnoreCase) && Port == obj2.Port;
        }

        public static bool operator ==(ServiceAddress obj1, ServiceAddress obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(ServiceAddress obj1, ServiceAddress obj2)
        {
            return !object.Equals(obj1, obj2);
        }

        public override string ToString()
        {
            if ((Uri.CheckHostName(Address) & UriHostNameType.IPv6) != 0)
                return "[" + Address + "]:" + Port;

            return Address + ":" + Port;
        }

        public static ServiceAddress Parse(string s)
        {
            Contract.Requires(s != null);

            int index = s.IndexOf(':'), port;
            if (index <= 0 || !int.TryParse(s.Substring(index + 1), out port))
                throw new FormatException("ServiceAddress格式错误:" + s);

            return new ServiceAddress(s.Substring(0, index).Trim('[', ']'), port);
        }

        public static bool TryParse(string s, out ServiceAddress serviceAddress)
        {
            int index, port;
            if (s == null || (index = s.IndexOf(':')) <= 0 || !int.TryParse(s.Substring(index + 1), out port))
            {
                serviceAddress = default(ServiceAddress);
                return false;
            }

            serviceAddress = new ServiceAddress(s.Substring(0, index).Trim('[', ']', ' '), port);
            return true;
        }

        public static bool IsCorrect(string s)
        {
            ServiceAddress address;
            return TryParse(s, out address);
        }

        private static readonly IPAddress _localhost = IPAddress.Parse("127.0.0.1");

        public bool IsLocalHost()
        {
            IPAddress address;
            return string.IsNullOrEmpty(Address)
                || (IPAddress.TryParse(Address, out address) && object.Equals(address, _localhost))
                || Address.ToLower() == "localhost";
        }
    }
}
