using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [UnitaryCalculate(typeof(float))]
    class FloatUnitaryCalculate : DefaultUnitaryCalculate
    {
        public override Value Minus(Value value)
        {
            return -(float)value;
        }

        public override Value Not(Value value)
        {
            float v = (float)value;
            int intV = (int)v;
            if (intV == v)
                return ~intV;

            return base.Not(value);
        }
    }
}
