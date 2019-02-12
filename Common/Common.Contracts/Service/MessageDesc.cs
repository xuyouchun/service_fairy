using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 消息描述
    /// </summary>
    [Serializable, DataContract]
    public class MessageDesc
    {
        public MessageDesc(string name, SVersion version)
        {
            Contract.Requires(name != null);

            Name = name;
            Version = version;
        }

        /// <summary>
        /// 消息名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 消息版本号
        /// </summary>
        [DataMember]
        public SVersion Version { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(MessageDesc))
                return false;

            MessageDesc sd = (MessageDesc)obj;
            return Name == sd.Name && Version == sd.Version;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Version.GetHashCode();
        }

        public static bool operator ==(MessageDesc sd1, MessageDesc sd2)
        {
            return object.Equals(sd1, sd2);
        }

        public static bool operator !=(MessageDesc sd1, MessageDesc sd2)
        {
            return !object.Equals(sd1, sd2);
        }

        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="full">当版本号为1.0时，是否需要显示</param>
        /// <returns></returns>
        public string ToString(bool full)
        {
            return (Version.IsEmpty || (!full && Version == SVersion.Version_1)) ? Name : (Name + " " + Version);
        }

        public static MessageDesc Parse(string s)
        {
            string name;
            SVersion version;

            _Parse(s, out name, out version);
            return new MessageDesc(name, version);
        }

        private static void _Parse(string s, out string name, out SVersion version)
        {
            int index;
            if (string.IsNullOrEmpty(s = s.Trim()))
            {
                name = string.Empty;
                version = SVersion.Empty;
            }
            else if ((index = s.IndexOf(' ')) > 0)
            {
                name = s.Substring(0, index);
                version = SVersion.Parse(s.Substring(index + 1));
            }
            else
            {
                name = s;
                version = SVersion.Empty;
            }
        }
    }
}
