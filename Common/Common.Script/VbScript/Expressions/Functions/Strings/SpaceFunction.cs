using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("Space", typeof(ValueTypes.IntegerType))]
    [FunctionInfo("返回指定长度的由空格组成的字符串", "length")]
    class SpaceFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            int num = values[0];
            if (num <= 0)
                return string.Empty;

            return string.Empty.PadRight(num, ' ');
        }
    }
}
