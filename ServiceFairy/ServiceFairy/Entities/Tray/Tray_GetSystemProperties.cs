using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts;
using Common.Utility;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取系统属性－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetSystemProperties_Request : RequestEntity
    {
        /// <summary>
        /// 系统属性，如果留空则返回全部系统属性
        /// </summary>
        [DataMember]
        public string[] Names { get; set; }
    }

    /// <summary>
    /// 系统属性
    /// </summary>
    public static class SystemPropertyNames
    {
        static SystemPropertyNames()
        {
            AllItems = typeof(SystemPropertyNames).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(f => f.IsDefined(typeof(SummaryAttribute), true))
                .Select(f => new PropertyItem((string)f.GetValue(null), SummaryAttribute.GetSummary(f, false)))
                .ToReadOnlyCollection();

            _dict = AllItems.ToDictionary(item => item.Name);
        }

        /// <summary>
        /// CPU信息
        /// </summary>
        [Summary("CPU信息")]
        public const string CpuInfo = "CpuInfo";

        /// <summary>
        /// 硬盘容量
        /// </summary>
        [Summary("硬盘容量")]
        public const string HdSize = "HdSize";

        /// <summary>
        /// 硬盘可用容量
        /// </summary>
        [Summary("硬盘可用容量")]
        public const string HdFreeSize = "HdFreeSize";

        /// <summary>
        /// 物理内存
        /// </summary>
        [Summary("物理内存")]
        public const string PhysicalMemorySize = "PhysicalMemorySize";

        /// <summary>
        /// 可用内存
        /// </summary>
        [Summary("可用内存")]
        public const string AvaliableMemorySize = "AvaliableMemorySize";

        /// <summary>
        /// 操作系统
        /// </summary>
        [Summary("操作系统")]
        public const string OpSysName = "OpSysName";

        public class PropertyItem
        {
            public PropertyItem(string name, string desc)
            {
                Name = name;
                Desc = desc;
            }

            public string Name { get; private set; }

            public string Desc { get; private set; }
        }

        /// <summary>
        /// 所有属性项
        /// </summary>
        public static readonly ReadOnlyCollection<PropertyItem> AllItems;

        private static readonly Dictionary<string, PropertyItem> _dict;

        /// <summary>
        /// 获取指定名称的属性项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyItem Get(string name)
        {
            Contract.Requires(name != null);

            return _dict.GetOrDefault(name);
        }
    }

    /// <summary>
    /// 系统属性项
    /// </summary>
    [Serializable, DataContract]
    public class SystemProperty
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }
    }

    /// <summary>
    /// 获取系统属性－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetSystemProperties_Reply : ReplyEntity
    {
        /// <summary>
        /// 属性
        /// </summary>
        [DataMember]
        public SystemProperty[] Properties { get; set; }
    }
}
