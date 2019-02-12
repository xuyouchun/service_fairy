using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Expressions.Functions.Strings
{
    /// <summary>
    /// 返回给定字符串的小写形式
    /// </summary>
    [Function("StrToLow",typeof(string))]
    class StrToLowFunction:FunctionBase
    {
        protected override Value OnExecute(RunningContext context, Value[] values)
        {
            return ((string)values[0]).ToLower();
        }
    }
}
