using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Common.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 通信配置
    /// </summary>
    [Serializable, DataContract]
    public class CommunicationOption
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="communicationType">协议</param>
        /// <param name="duplex">单向或双向</param>
        [JsonConstructor]
        public CommunicationOption(ServiceAddress address, CommunicationType communicationType = CommunicationType.WTcp, bool duplex = false)
        {
            Address = address;
            Type = communicationType;
            Duplex = duplex;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="communicationType">协议</param>
        /// <param name="duplex">单向或双向</param>
        public CommunicationOption(string address, CommunicationType communicationType = CommunicationType.WTcp, bool duplex = false)
            : this(ServiceAddress.Parse(address), communicationType, duplex)
        {

        }

        /// <summary>
        /// 终端
        /// </summary>
        [DataMember]
        public ServiceAddress Address { get; private set; }

        /// <summary>
        /// 通信方式
        /// </summary>
        [DataMember]
        public CommunicationType Type { get; private set; }

        /// <summary>
        /// 是否支持双向通信
        /// </summary>
        [DataMember]
        public bool Duplex { get; private set; }

        /// <summary>
        /// 将字符串转换为CommunicationOption
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static CommunicationOption Parse(string s)
        {
            Contract.Requires(s != null);

            CommunicationOption option;
            string errorMsg = _TryParse(s, out option);
            if (errorMsg != null)
                throw new FormatException(errorMsg);

            return option;
        }

        private static string _TryParse(string s, out CommunicationOption value)
        {
            value = null;
            if (s == null || (s = s.Trim()).Length == 0)
            {
                value = null;
                return "字符串为空串或空引用";
            }

            const string PREFIX = "net.", SPLITTER = "://";
            if (s.StartsWith(PREFIX, StringComparison.OrdinalIgnoreCase))  // 类似 “net.tcp://127.0.0.1:80/d”的格式
            {
                int index = s.IndexOf(SPLITTER);
                if (index < 0)
                    return "协议格式不正确";

                string sType = s.Substring(PREFIX.Length, index - PREFIX.Length);
                CommunicationType type;
                if (!Enum.TryParse<CommunicationType>(sType, true, out type))
                    return string.Format("协议{0}不被识别", sType);

                int addressEndIndex = s.IndexOf('/', index + SPLITTER.Length);
                string sAddress = (addressEndIndex < 0) ? s.Substring(index + SPLITTER.Length)
                    : s.Substring(index + SPLITTER.Length, addressEndIndex - index - SPLITTER.Length);

                ServiceAddress address;
                if (!ServiceAddress.TryParse(sAddress, out address))
                    return string.Format("服务地址{0}格式不正确", address);

                bool duplex = (addressEndIndex >= 0 && s.Substring(addressEndIndex + 1).ToLower() == "d");
                value = new CommunicationOption(address, type, duplex);
                return null;
            }
            else  // 类似 “TCP 127.0.0.1:80 双向”的格式
            {
                value = null;
                string[] parts = Regex.Split(s, @"\s+");
                ServiceAddress address;
                CommunicationType type = CommunicationType.WTcp;
                bool supportDuplex = false;

                if (ServiceAddress.TryParse(parts[0], out address))  // 地址在前的情况
                {
                    if (parts.Length > 1)
                    {
                        if (!Enum.TryParse<CommunicationType>(parts[1], true, out type))
                            return "协议格式不正确: " + parts[1];

                        if (type == CommunicationType.Unknown)
                            type = CommunicationType.WTcp;
                    }
                }
                else // 地址在后的情况
                {
                    if (parts.Length < 2)
                        return "格式不完整";

                    if (!Enum.TryParse<CommunicationType>(parts[0], true, out type))
                        return "协议格式不正确:" + parts[0];

                    if (type == CommunicationType.Unknown)
                        type = CommunicationType.WTcp;

                    address = ServiceAddress.Parse(parts[1]);
                }

                if (parts.Length > 2)
                {
                    if (parts[2] == "单向")
                        supportDuplex = false;
                    else if (parts[2] == "双向")
                        supportDuplex = true;
                    else
                        bool.TryParse(parts[2], out supportDuplex);
                }

                value = new CommunicationOption(address, type, supportDuplex);
                return null;
            }
        }

        /// <summary>
        /// 将字符串转换为CommunicationOption
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out CommunicationOption value)
        {
            return _TryParse(s, out value) == null;
        }

        /// <summary>
        /// 是否为正确的格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsCorrect(string s)
        {
            CommunicationOption op;
            return TryParse(s, out op);
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode() ^ Type.GetHashCode() ^ Duplex.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(CommunicationOption))
                return false;

            CommunicationOption obj2 = (CommunicationOption)obj;
            return obj2.Address == Address && obj2.Type == Type && obj2.Duplex == Duplex;
        }

        public override string ToString()
        {
            return Type.ToString().ToUpper() + " " + Address + " " + (Duplex ? "双向" : "单向");
        }

        /// <summary>
        /// 转换为url格式
        /// </summary>
        /// <returns></returns>
        public string ToString(bool urlFormat)
        {
            if (!urlFormat)
                return ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("net.").Append(Type.ToString().ToLower()).Append("://").Append(Address.ToString());
            if (Duplex)
                sb.Append("/d");

            return sb.ToString();
        }

        public static bool operator ==(CommunicationOption obj1, CommunicationOption obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(CommunicationOption obj1, CommunicationOption obj2)
        {
            return !object.Equals(obj1, obj2);
        }

        public static readonly IEqualityComparer<CommunicationOption> Comparer = new CommunicationOptionEqualityComparer();

        #region Class CommunicationOptionEqualityComparer ...

        private class CommunicationOptionEqualityComparer : IEqualityComparer<CommunicationOption>
        {
            public bool Equals(CommunicationOption x, CommunicationOption y)
            {
                if (x == null || y == null)
                    return x == null && y == null;

                return object.Equals(x.Address, y.Address);
            }

            public int GetHashCode(CommunicationOption obj)
            {
                return (obj == null || obj.Address == null) ? 0 : obj.Address.GetHashCode();
            }
        }

        #endregion

        public bool IsLocalHost()
        {
            return Address == null || Address.IsLocalHost();
        }
    }
}
