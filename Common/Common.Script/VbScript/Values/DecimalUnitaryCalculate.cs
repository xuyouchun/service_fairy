using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [UnitaryCalculate(typeof(decimal))]
    class DecimalUnitaryCalculate : DefaultUnitaryCalculate
    {
        public override Value Minus(Value value)
        {
            return -(decimal)value;
        }

        public override Value Not(Value value)
        {
            decimal v = (decimal)value;
            if ((long)v == v)
            {
                return ~(long)v;
            }

            return base.Not(value);
        }
    }
}
