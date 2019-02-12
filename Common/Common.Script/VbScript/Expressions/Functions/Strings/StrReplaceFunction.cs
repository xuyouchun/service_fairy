using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Expressions.Functions.Strings
{
    /// <summary>
    /// 替换给定字符串中的指定字符
    /// 两个参数：OldString，NewString
    /// </summary>
    [Function("StrReplace",typeof(string),typeof(string),typeof(string))]
    class StrReplaceFunction:FunctionBase
    {
        protected override Value OnExecute(RunningContext context, Value[] values)
        {
            string resource = values[0].ToString();
            return (resource.Replace(values[1].ToString(), values[2].ToString()));
        }
    }
}
