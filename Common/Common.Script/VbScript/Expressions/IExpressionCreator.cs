using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 表达式的创建器
    /// </summary>
    interface IExpressionCreator
    {
        /// <summary>
        /// 创建表达式
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Expression Create(Expression[] parameters);
    }
}
