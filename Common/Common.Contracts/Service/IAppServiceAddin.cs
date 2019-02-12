using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 服务的插件
    /// </summary>
    public interface IAppServiceAddin : IDisposable
    {
        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <returns></returns>
        AppServiceAddinInfo GetInfo();

        /// <summary>
        /// 通信策略
        /// </summary>
        ICommunicate Communicate { get; }
    }

    /// <summary>
    /// 插件的信息
    /// </summary>
    [Serializable, DataContract]
    public class AppServiceAddinInfo
    {
        public AppServiceAddinInfo(AddinDesc addinDesc, string title, string desc)
        {
            Contract.Requires(addinDesc != null && title != null && desc != null);

            AddinDesc = addinDesc;
            Title = title;
            Desc = desc;
        }

        /// <summary>
        /// 名称与版本号
        /// </summary>
        [DataMember]
        public AddinDesc AddinDesc { get; private set; }

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
    }

    /// <summary>
    /// 插件描述
    /// </summary>
    [Serializable, DataContract]
    public class AddinDesc
    {
        public AddinDesc(string name, SVersion version)
        {
            Contract.Requires(name != null);

            Name = name;
            Version = version;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [DataMember]
        public SVersion Version { get; private set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Version.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(AddinDesc))
                return false;

            AddinDesc ad = (AddinDesc)obj;
            return Name == ad.Name && Version == ad.Version;
        }

        public static bool operator ==(AddinDesc obj1, AddinDesc obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(AddinDesc obj1, AddinDesc obj2)
        {
            return !object.Equals(obj1, obj2);
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
        /// 尝试从字符串转换
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out AddinDesc value)
        {
            if (string.IsNullOrEmpty(s))
                goto _false;

            int k = s.IndexOf(' ');
            if (k < 0)
            {
                value = new AddinDesc(s.Trim(), SVersion.Empty);
            }
            else
            {
                SVersion ver;
                if(!SVersion.TryParse(s.Substring(k+1), out ver))
                    goto _false;

                value = new AddinDesc(s.Substring(0, k).Trim(), ver);
            }

            return true;

        _false:
            value = null;
            return false;
        }

        public static AddinDesc Parse(string s)
        {
            Contract.Requires(s != null);

            AddinDesc value;
            if (!TryParse(s, out value))
                throw new FormatException("插件描述符的格式错误");

            return value;
        }
    }

    /// <summary>
    /// 用于标注AppService插件
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AppServiceAddinAttribute : Attribute
    {
        public AppServiceAddinAttribute(string name, string version, string title = "", string desc = "")
        {
            Contract.Requires(name != null && version != null);

            Name = name;
            Version = version;
            Title = title;
            Desc = desc;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public SVersion Version { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; private set; }

        public AppServiceAddinInfo ToAddinInfo()
        {
            return new AppServiceAddinInfo(new AddinDesc(Name, Version), Title, Desc);
        }
    }
}
