using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;
using Common.Contracts.Log;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace ServiceFairy.Entities.Tray
{
    /// <summary>
    /// 基于字符串表的日志项
    /// </summary>
    [Serializable, DataContract]
    public class StLogItem
    {
        /// <summary>
        /// 类别
        /// </summary>
        [DataMember]
        public int Category { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [DataMember]
        public MessageType Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public int Title { get; set; }

        /// <summary>
        /// 详细
        /// </summary>
        [DataMember]
        public int Detail { get; set; }

        /// <summary>
        /// 源
        /// </summary>
        [DataMember]
        public int Source { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 从LogItem转换
        /// </summary>
        /// <param name="logItem"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public static StLogItem From(LogItem logItem, StringTable st)
        {
            Contract.Requires(st != null);

            if (logItem == null)
                return null;

            return new StLogItem() {
                Category = st.AddString(logItem.Category),
                Title = st.AddString(logItem.Title),
                Detail = st.AddString(logItem.Detail),
                Source = st.AddString(logItem.Source),
                Type = logItem.Type,
                Time = logItem.Time,
            };
        }

        /// <summary>
        /// 批量转换
        /// </summary>
        /// <param name="logItems"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public static StLogItem[] From(IEnumerable<LogItem> logItems, ref StringTable st)
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
        public static StLogItem[] From(IEnumerable<LogItem> logItems, StringTable st)
        {
            return From(logItems, ref st);
        }

        /// <summary>
        /// 转换为LogItem
        /// </summary>
        /// <param name="stLogItem"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public LogItem ToLogItem(StringTable st)
        {
            Contract.Requires(st != null);

            return new LogItem(st.GetString(Category),
                Type, st.GetString(Title), st.GetString(Detail), st.GetString(Source), Time);
        }

        /// <summary>
        /// 转换为LogItem
        /// </summary>
        /// <param name="stLogItems"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public static LogItem[] ToLogItems(IEnumerable<StLogItem> stLogItems, StringTable st)
        {
            Contract.Requires(stLogItems != null && st != null);
            return stLogItems.ToArray(li => li.ToLogItem(st));
        }

        /// <summary>
        /// 转换为LogItem
        /// </summary>
        /// <param name="stLogItems"></param>
        /// <param name="stringTable"></param>
        /// <returns></returns>
        public static LogItem[] ToLogItems(IEnumerable<StLogItem> stLogItems, string[] stringTable)
        {
            return ToLogItems(stLogItems, new StringTable(stringTable));
        }
    }
}
