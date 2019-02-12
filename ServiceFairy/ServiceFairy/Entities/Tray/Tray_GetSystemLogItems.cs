using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Contracts;
using Common.Utility;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 获取系统日志－请求
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetSystemLogItems_Request : RequestEntity
    {
        /// <summary>
        /// 组名
        /// </summary>
        [DataMember]
        public string Group { get; set; }

        /// <summary>
        /// 起始
        /// </summary>
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public int Count { get; set; }
    }

    /// <summary>
    /// 系统日志（用于网络传输的压缩形式）
    /// </summary>
    [Serializable, DataContract]
    public class StSystemLogItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public int Message { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        [DataMember]
        public EventLogEntryType Type { get; set; }

        /// <summary>
        /// 源
        /// </summary>
        [DataMember]
        public int Source { get; set; }

        /// <summary>
        /// 从SystemLogItem转换
        /// </summary>
        /// <param name="StSystemLogItem"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public static StSystemLogItem From(SystemLogItem logItem, StringTable st)
        {
            Contract.Requires(st != null);

            if (logItem == null)
                return null;

            return new StSystemLogItem() {
                Message = st.AddString(logItem.Message),
                Time = logItem.Time,
                Type = logItem.Type,
                Source = st.AddString(logItem.Source),
            };
        }

        /// <summary>
        /// 批量转换
        /// </summary>
        /// <param name="logItems"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public static StSystemLogItem[] From(IEnumerable<SystemLogItem> logItems, ref StringTable st)
        {
            Contract.Requires(logItems != null);
            if (st == null)
                st = new StringTable();

            StringTable st0 = st;
            return logItems.ToArray(li => From(li, st0));
        }

        /// <summary>
        /// 批量转换
        /// </summary>
        /// <param name="logItems"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public static StSystemLogItem[] From(IEnumerable<SystemLogItem> logItems, StringTable st)
        {
            return From(logItems, ref st);
        }

        /// <summary>
        /// 转换为StSystemLogItem
        /// </summary>
        /// <param name="StSystemLogItem"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public SystemLogItem ToSystemLogItem(StringTable st)
        {
            Contract.Requires(st != null);

            return new SystemLogItem {
                Type = Type, Time = Time, Message = st.GetString(Message), Source = st.GetString(Source),
            };
        }

        /// <summary>
        /// 转换为StSystemLogItem
        /// </summary>
        /// <param name="logItems"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public static SystemLogItem[] ToSystemLogItems(IEnumerable<StSystemLogItem> logItems, StringTable st)
        {
            Contract.Requires(logItems != null && st != null);
            return logItems.ToArray(li => li.ToSystemLogItem(st));
        }

        /// <summary>
        /// 转换为SystemLogItem
        /// </summary>
        /// <param name="logItems"></param>
        /// <param name="stringTable"></param>
        /// <returns></returns>
        public static SystemLogItem[] ToSystemLogItems(IEnumerable<StSystemLogItem> logItems, string[] stringTable)
        {
            return ToSystemLogItems(logItems, new StringTable(stringTable));
        }
    }

    /// <summary>
    /// 系统日志
    /// </summary>
    [Serializable, DataContract]
    public class SystemLogItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        [DataMember]
        public EventLogEntryType Type { get; set; }

        /// <summary>
        /// 源
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }

    /// <summary>
    /// 获取系统日志－应答
    /// </summary>
    [Serializable, DataContract]
    public class Tray_GetSystemLogItems_Reply : ReplyEntity
    {
        /// <summary>
        /// 字符串表
        /// </summary>
        [DataMember]
        public string[] StringTable { get; set; }

        /// <summary>
        /// 日志
        /// </summary>
        [DataMember]
        public StSystemLogItem[] LogItems { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }
    }
}
