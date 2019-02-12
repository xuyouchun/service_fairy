using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 用于控制脚本的执行
    /// </summary>
    public interface IScriptExecutorAsyncResult : IAsyncResult
    {
        /// <summary>
        /// 暂停执行
        /// </summary>
        void Pause();

        /// <summary>
        /// 恢复执行
        /// </summary>
        void Resume();

        /// <summary>
        /// 是否在运行
        /// </summary>
        bool Running { get; }

        /// <summary>
        /// 中止运行
        /// </summary>
        void Stop();

        /// <summary>
        /// 获取变量的值
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="result"></param>
        /// <param name="expType"></param>
        /// <returns></returns>
        bool GetValue(string exp, out Value result, out ExpressionType expType);
    }
}
