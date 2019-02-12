using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.IO;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 用于标注AppService类的一些信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false), Serializable]
    public class AppServiceAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="version">版本号</param>
        /// <param name="title">标题</param>
        /// <param name="desc">描述</param>
        /// <param name="defaultDataFormat">默认的编码方式</param>
        /// <param name="defaultBufferType">默认的数据缓冲区格式</param>
        /// <param name="weight">权重</param>
        public AppServiceAttribute(string name, string version = "1.0", string title = "", string desc = "",
            DataFormat defaultDataFormat = DataFormat.Binary, BufferType defaultBufferType = BufferType.Bytes, int weight = int.MaxValue, AppServiceCategory category = AppServiceCategory.Application)
        {
            Name = name ?? string.Empty;
            Title = title ?? string.Empty;
            Desc = desc ?? string.Empty;
            Version = SVersion.Parse(version);
            DefaultDataFormat = defaultDataFormat;
            DefaultBufferType = defaultBufferType;
            Weight = weight;
            Category = category;
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

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; private set; }

        /// <summary>
        /// 组别
        /// </summary>
        public AppServiceCategory Category { get; private set; }

        /// <summary>
        /// 默认的编码方式
        /// </summary>
        public DataFormat DefaultDataFormat { get; private set; }

        /// <summary>
        /// 默认的数据缓冲区格式
        /// </summary>
        public BufferType DefaultBufferType { get; private set; }

        public override string ToString()
        {
            return Version.IsEmpty ? Name : (Name + " " + Version);
        }
    }

    /// <summary>
    /// 服务的组别
    /// </summary>
    public enum AppServiceCategory
    {
        /// <summary>
        /// 核心服务
        /// </summary>
        [Desc("核心服务")]
        Core = 2,

        /// <summary>
        /// 系统服务
        /// </summary>
        [Desc("系统服务")]
        System = 1,

        /// <summary>
        /// 应用服务
        /// </summary>
        [Desc("应用服务")]
        Application = 0,
    }
}
