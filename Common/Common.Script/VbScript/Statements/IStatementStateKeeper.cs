using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 用于保存住当前的运行状态
    /// </summary>
    interface IStatementStateKeeper
    {
        /// <summary>
        /// 保存当前的运行状态
        /// </summary>
        /// <returns></returns>
        object KeepState();

        /// <summary>
        /// 设置当前的运行状态
        /// </summary>
        /// <param name="state"></param>
        void SetState(object state);

        /// <summary>
        /// 获取下一个语句块
        /// </summary>
        /// <returns></returns>
        IStatementStateKeeper GetNext();
    }
}
