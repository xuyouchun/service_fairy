using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 服务的描述
    /// </summary>
    [Serializable, DataContract]
    public class ServiceDesc : IComparable<ServiceDesc>
    {
        public ServiceDesc()
            : this("", SVersion.Empty)
        {

        }

        public ServiceDesc(string name, SVersion version)
        {
            Contract.Requires(name != null);

            Name = name;
            Version = version;
        }

        public ServiceDesc(string name)
            : this(name, SVersion.Empty)
        {

        }

        /// <summary>
        /// 服务的名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 服务的版本号
        /// </summary>
        [DataMember]
        public SVersion Version { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ServiceDesc))
                return false;

            ServiceDesc sd = (ServiceDesc)obj;
            return Name == sd.Name && Version == sd.Version;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Version.GetHashCode();
        }

        public static bool operator ==(ServiceDesc sd1, ServiceDesc sd2)
        {
            return object.Equals(sd1, sd2);
        }

        public static bool operator !=(ServiceDesc sd1, ServiceDesc sd2)
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

        /// <summary>
        /// 判断两个服务描述是否兼容
        /// </summary>
        /// <param name="sd1"></param>
        /// <param name="sd2"></param>
        /// <returns></returns>
        public static bool Compatible(ServiceDesc sd1, ServiceDesc sd2)
        {
            if (sd1 == null || sd2 == null)
                return false;

            return sd1.Name == sd2.Name && (sd1.Version.IsEmpty || sd2.Version.IsEmpty || sd1.Version == sd2.Version);
        }

        public static ServiceDesc Parse(string s)
        {
            return MethodParser.ParseService(s);
        }

        public int CompareTo(ServiceDesc other)
        {
            if (other == null)
                return 1;

            int v = Name.CompareTo(other.Name);
            if (v == 0)
                v = Version.CompareTo(other.Version);

            return v;
        }
    }
}
