using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 用于支持goto语句
    /// </summary>
    interface IGotoSupported
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statement">从哪条语句开始执行</param>
        /// <returns></returns>
        void Execute(RunningContext context, Statement statement);
    }
}
