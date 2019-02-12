using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 可支持左值的表达式
    /// </summary>
    interface ILeftValueExpression
    {
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        void SetValue(IExpressionContext context, Value value);
    }
}
