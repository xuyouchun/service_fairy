using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Expressions.Functions.Strings
{
    /// <summary>
    /// 返回给定字符串的大写形式
    /// </summary>
    [Function("StrToUp",typeof(string))]
    class StrToUpFunction:FunctionBase
    {
        protected override Value OnExecute(RunningContext context, Value[] values)
        {
            return ((string)values[0]).ToUpper();
        }
    }
}
