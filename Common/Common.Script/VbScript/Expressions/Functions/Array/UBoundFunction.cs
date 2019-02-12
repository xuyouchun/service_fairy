using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;

namespace Common.Script.VbScript.Expressions.Functions.Array
{
    [Function("UBound", false, typeof(ArrayObject))]
    [FunctionInfo("返回指定数组指定维数的长度", "array [,rank]")]
    class UBoundFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            if (values.Length > 2)
                throw new ScriptRuntimeException("获取数组维数时指定的参数个数错误");

            ArrayObject array = (ArrayObject)values[0];
            int bound;
            if (values.Length == 1)
            {
                bound = 1;
            }
            else
            {
                bound = ConvertToInteger(values[1]);
                if (bound < 1)
                    throw new ScriptRuntimeException("获取数组维数时必须指定大于零的参数");

                if (bound > array.GetRank())
                    throw new ScriptRuntimeException("获取数组维数时指定的参数超出范围");
            }

            return array.GetRankLength(bound) - 1;
        }
    }
}
