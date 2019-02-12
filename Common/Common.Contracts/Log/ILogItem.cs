using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Log
{
    /// <summary>
    /// 日志项
    /// </summary>
    public interface ILogItem
    {
        /// <summary>
        /// 类别
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// 产生日志的源
        /// </summary>
        string Source { get; }

        /// <summary>
        /// 日志类型
        /// </summary>
        MessageType Type { get; }

        /// <summary>
        /// 记录时间
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// 信息
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        string ToText();
    }
}
