using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [UnitaryCalculate(typeof(double))]
    class DoubleUnitaryCalculate : DefaultUnitaryCalculate
    {
        public override Value Minus(Value value)
        {
            return -(double)value;
        }

        public override Value Not(Value value)
        {
            double v = (double)value;
            long longV = (long)v;

            if (longV == v)
                return ~longV;

            return base.Not(value);
        }
    }
}
