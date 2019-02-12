using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;

namespace Common.Script.VbScript.Expressions.Functions.Array
{
    [Function("Array", false)]
    [FunctionInfo("创建一个数组", "[bound1 [,bould2 ...]]")]
    class ArrayFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            int[] upBounds = ConvertToIntegers(values);

            return new ArrayObject(upBounds);
        }
    }
}
