using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 表达式编译时的上下文环境
    /// </summary>
    public interface IExpressionCompileContext
    {
        Value GetConstValue(string name);
    }
}
