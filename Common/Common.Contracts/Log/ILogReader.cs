using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 日志读取器
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    public interface ILogReader<out TLogItem>
        where TLogItem : class, ILogItem
    {
        /// <summary>
        /// 读取指定日志组中的所有日志组
        /// </summary>
        /// <param name="parentGroup">父日志组</param>
        /// <returns></returns>
        LogItemGroup[] ReadGroups(string parentGroup);

        /// <summary>
        /// 读取指定时间范围内的日志组
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="end">结束</param>
        /// <returns></returns>
        LogItemGroup[] ReadGroupsByTime(DateTime start, DateTime end);

        /// <summary>
        /// 读取指定日志组中的所有日志
        /// </summary>
        /// <param name="parentGroup">父日志组</param>
        /// <returns></returns>
        TLogItem[] ReadItems(string parentGroup);

        /// <summary>
        /// 删除日志组
        /// </summary>
        /// <param name="group">日志组</param>
        void DeleteGroup(string group);
    }

    /// <summary>
    /// 日志组
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    [Serializable, DataContract]
    public class LogItemGroup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="creationTime"></param>
        /// <param name="size"></param>
        public LogItemGroup(string name, DateTime creationTime, DateTime lastModifyTime, long size)
        {
            Name = name;
            CreationTime = creationTime;
            LastModifyTime = lastModifyTime;
            Size = size;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DataMember]
        public DateTime LastModifyTime { get; private set; }

        /// <summary>
        /// 大小
        /// </summary>
        [DataMember]
        public long Size { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
