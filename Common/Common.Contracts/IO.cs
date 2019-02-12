using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Contracts
{
    /// <summary>
    /// 输入窗口
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// 读取一串字符串
        /// </summary>
        /// <returns></returns>
        string Read();
    }

    /// <summary>
    /// 输出窗口
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// 输出一串字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        void Write(string s, MessageType type = MessageType.Message);
    }

    /// <summary>
    /// 空的输出窗口
    /// </summary>
    public class EmptyOutput : IOutput
    {
        public void Write(string s, MessageType type = MessageType.Message)
        {
            
        }
    }

    public static class OutputHelper
    {
        public static void WriteLine(this IOutput output, string s, MessageType type = MessageType.Message)
        {
            Contract.Requires(output != null);
            output.Write(s + "\r\n", type);
        }

        public static void WriteFormat(this IOutput output, string format, object[] parameters)
        {
            Contract.Requires(output != null && format != null);
            WriteFormat(output, format, MessageType.Message, parameters);
        }

        public static void WriteFormat(this IOutput output, string format, MessageType type, object[] parameters)
        {
            Contract.Requires(output != null && format != null);
            output.Write(string.Format(format, parameters), type);
        }
    }

    /// <summary>
    /// 基于控制台窗口的输入输出
    /// </summary>
    public class ConsoleInputOutput : IInput, IOutput
    {
        public string Read()
        {
            return Console.ReadLine();
        }

        public void Write(string s, MessageType type)
        {
            Console.Write(s);
        }
    }

    /// <summary>
    /// 信息类型
    /// </summary>
    public enum MessageType
    {
        [Desc("未知")]
        Unknown = 0,

        /// <summary>
        /// 普通消息
        /// </summary>
        [Desc("信息")]
        Message = 1,

        /// <summary>
        /// 警告
        /// </summary>
        [Desc("警告")]
        Warning = 20,

        /// <summary>
        /// 错误
        /// </summary>
        [Desc("错误")]
        Error = 30,
    }

    /// <summary>
    /// 属性显示器
    /// </summary>
    public interface IPropertyViewer
    {
        /// <summary>
        /// 显示指定对象的属性
        /// </summary>
        /// <param name="obj"></param>
        void ShowProperty(object obj);
    }
}
