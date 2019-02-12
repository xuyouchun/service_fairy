using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions.ExpressionAnalysers;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 分析器
    /// </summary>
    class ExpressionAnalyser
    {
        public ExpressionAnalyser()
        {
            
        }

        /// <summary>
        /// 从字符串中加载表达式
        /// </summary>
        /// <param name="expStr"></param>
        /// <param name="compileContext"></param>
        /// <returns></returns>
        public unsafe Expression[] Parse(string expStr, IExpressionCompileContext compileContext)
        {
            if (expStr == null)
                throw new ArgumentNullException("expStr");

            fixed (char* ptr = expStr)
            {
                ExpressionAnalyserExecutor executor = new ExpressionAnalyserExecutor(ptr, compileContext);
                return executor.Parse();
            }
        }
        
    }
}
