using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Abs", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求绝对值", "num")]
    class AbsFunction:FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            Value v = values[0];
            if (v.InnerValue == null)
                return Value.Void;

            Type t = v.InnerValue.GetType();
            if (t == typeof(double))
                return Math.Abs((double)v);
            else if (t == typeof(int))
                return Math.Abs((int)v);
            else if (t == typeof(decimal))
                return Math.Abs((decimal)v);
            else if (t == typeof(float))
                return Math.Abs((float)v);
            else if (t == typeof(long))
                return Math.Abs((long)v);
            else if (t == typeof(short))
                return Math.Abs((short)v);
            else if (t == typeof(sbyte))
                return Math.Abs((sbyte)v);

            return Math.Abs((double)v);
        }
    }
}
