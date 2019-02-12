using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 日志项
    /// </summary>
    [Serializable, DataContract, ObjectCreator(typeof(Creator))]
    public class LogItem : ILogItem
    {
        public LogItem(string category, MessageType type, string title, string detail = "", string source = "", DateTime time = default(DateTime))
        {
            Category = category;
            Source = source;
            Type = type;
            Title = title ?? string.Empty;
            Detail = detail ?? string.Empty;
            Time = (time == default(DateTime)) ? DateTime.UtcNow : time;
        }

        #region ILogItem Members

        /// <summary>
        /// 唯一标识
        /// </summary>
        [DataMember]
        public string Category { get; set; }

        /// <summary>
        /// 产生记录的源
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        [DataMember]
        public MessageType Type { get; set; }

        /// <summary>
        /// 日志记录时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        [DataMember]
        public string Detail { get; set; }

        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[").Append(Time.ToString("yyyy-MM-dd HH:mm:ss"))
                .Append("] ").Append(Type)
                .Append(" ").Append(Title);

            if (!string.IsNullOrWhiteSpace(Detail))
                sb.Append(" ").Append(Detail);

            if (!string.IsNullOrEmpty(Source))
                sb.Append(" <").Append(Source).Append(">");

            return sb.ToString();
        }

        public string ToText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Category).Append("\t");
            sb.Append(Time.ToString()).Append("\t");
            sb.Append(Type).Append("\t");
            sb.Append(Title.Replace('\t', ' ')).Append("\t");
            sb.Append(Detail.Replace('\t', ' ')).Append("\t");
            sb.Append(Source);

            return sb.ToString();
        }

        public static readonly LogItem Empty = new LogItem("", MessageType.Unknown, "");

        #region Class Creator ...

        class Creator : IObjectCreator
        {
            public object CreateObject(object context = null, IObjectCreator innerCreator = null)
            {
                string s = context as string;
                string[] ss;
                if (string.IsNullOrEmpty(s) || (ss = s.Split('\t')).Length < 6)
                    return Empty;

                string category = ss[0];

                DateTime dt;
                DateTime.TryParse(ss[1], out dt);

                MessageType msgType = MessageType.Unknown;
                Enum.TryParse<MessageType>(ss[2], true, out msgType);

                string title = ss[3], detail = ss[4], source = ss[5];
                return new LogItem(category, msgType, title, detail, source, dt);
            }
        }

        #endregion

        /// <summary>
        /// 创建错误日志
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="error">异常</param>
        /// <param name="source">源</param>
        public static LogItem FromError(string category, Exception error, string source = "")
        {
            Contract.Requires(error != null);

            return FromError(category, error.Message, error.StackTrace, source);
        }

        /// <summary>
        /// 创建错误日志
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="title">标题</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static LogItem FromError(string category, string title, string detail = "", string source = "")
        {
            return new LogItem(category, MessageType.Error, title, detail, source);
        }

        /// <summary>
        /// 创建错误日志
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="titleFormat">标题格式</param>
        /// <param name="args">标题参数</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static LogItem FromErrorFormat(string category, string titleFormat, object[] args, string detail = "", string source = "")
        {
            Contract.Requires(titleFormat != null);
            return FromError(category, string.Format(titleFormat, args ?? Array<object>.Empty), detail, source);
        }

        /// <summary>
        /// 创建错误日志
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="titleFormat">标题字符串格式</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static LogItem FromErrorFormat(string category, string titleFormat, params object[] args)
        {
            return FromErrorFormat(category, titleFormat, args, "", "");
        }

        /// <summary>
        /// 创建警告日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static LogItem FromWarning(string category, string title, string detail = "", string source = "")
        {
            return new LogItem(category, MessageType.Warning, title, detail, source);
        }

        /// <summary>
        /// 创建警告日志
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="titleFormat">标题格式</param>
        /// <param name="args">标题参数</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static LogItem FromWarningFormat(string category, string titleFormat, object[] args, string detail = "", string source = "")
        {
            Contract.Requires(titleFormat != null);

            return FromWarning(category, string.Format(titleFormat, args ?? Array<string>.Empty), detail, source);
        }

        /// <summary>
        /// 创建警告日志
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="titleFormat">标题格式</param>
        /// <param name="args">标题参数</param>
        /// <returns></returns>
        public static LogItem FromWarningFormat(string category, string titleFormat, params object[] args)
        {
            return FromWarningFormat(category, titleFormat, args, "", "");
        }

        /// <summary>
        /// 从普通消息创建
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="title">标题</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static LogItem FromMessage(string category, string title, string detail = "", string source = "")
        {
            return new LogItem(category, MessageType.Message, title, detail, source);
        }

        /// <summary>
        /// 从普通消息创建
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="titleFormat">标题格式</param>
        /// <param name="args">标题参数</param>
        /// <param name="detail">细节</param>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static LogItem FromMessageFormat(string category, string titleFormat, object[] args, string detail = "", string source = "")
        {
            Contract.Requires(titleFormat != null);

            return FromMessage(category, string.Format(titleFormat, args ?? Array<object>.Empty), detail, source);
        }

        /// <summary>
        /// 从普通消息创建
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="titleFormat">标题格式</param>
        /// <param name="args">标题参数</param>
        /// <returns></returns>
        public static LogItem FromMessageFormat(string category, string titleFormat, params object[] args)
        {
            return FromMessageFormat(category, titleFormat, args, "", "");
        }
    }
}
